"""Generate .ico from .svg using skia-python + Pillow (like ControlNav)."""
import io, os, sys, tempfile, shutil, struct
from PIL import Image
import skia

def svg_to_pil(svg_path, width, height):
    with open(svg_path, 'rb') as f:
        svg_data = f.read()
    stream = skia.MemoryStream(svg_data)
    svg_dom = skia.SVGDOM.MakeFromStream(stream)
    if not svg_dom:
        return None
    svg_dom.setContainerSize(skia.Size(512, 512))
    surface = skia.Surface(512, 512)
    with surface as canvas:
        canvas.clear(skia.Color(0, 0, 0, 0))
        svg_dom.render(canvas)
    image = surface.makeImageSnapshot()
    buf = io.BytesIO(image.encodeToData())
    pil = Image.open(buf).copy()
    buf.close()
    if width != 512 or height != 512:
        pil = pil.resize((width, height), Image.LANCZOS)
    return pil.convert('RGBA')

def generate_icon(svg_path, ico_path, sizes=(256, 128, 64, 48, 32, 16)):
    # Render all sizes to PNG in memory
    images = []
    for s in sorted(sizes, reverse=True):
        pil_img = svg_to_pil(svg_path, s, s)
        if pil_img is None:
            raise RuntimeError(f"Failed to render SVG at {s}px")
        images.append(pil_img)

    # Write ICO manually (reliable, no Pillow append_images issues)
    with open(ico_path, 'wb') as f:
        # ICO header
        f.write(struct.pack('<HHH', 0, 1, len(images)))  # reserved, type=ico, count

        offset = 6 + 16 * len(images)
        png_data = []
        for img in images:
            w, h = img.size
            buf = io.BytesIO()
            img.save(buf, 'PNG')
            png_bytes = buf.getvalue()
            png_data.append(png_bytes)

            # Directory entry
            f.write(struct.pack('<BBBBHHII',
                w if w < 256 else 0,
                h if h < 256 else 0,
                0,  # color palette
                0,  # reserved
                1,  # color planes
                32, # bits per pixel
                len(png_bytes),
                offset))
            offset += len(png_bytes)

        # PNG data
        for png in png_data:
            f.write(png)
    print(f"ICO created with {len(images)} frames")

if __name__ == '__main__':
    if len(sys.argv) != 3:
        print(f"Usage: {sys.argv[0]} <input.svg> <output.ico>", file=sys.stderr)
        sys.exit(1)
    generate_icon(sys.argv[1], sys.argv[2])
"""Generate .ico from .svg using skia-python + Pillow (like ControlNav)."""
import io, os, sys, tempfile, shutil
from PIL import Image
import skia

def svg_to_png(svg_path, width, height):
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
    return pil.resize((width, height), Image.LANCZOS)

def generate_icon(svg_path, ico_path, sizes=(256, 128, 64, 48, 32, 16)):
    tmpdir = tempfile.mkdtemp(prefix='icon_')
    try:
        images = []
        for s in sorted(sizes, reverse=True):
            png = svg_to_png(svg_path, s, s)
            if png is None:
                raise RuntimeError(f"Failed to render SVG at {s}px")
            images.append(png.convert('RGBA'))
        images[0].save(
            ico_path, format='ICO',
            sizes=[(img.width, img.height) for img in images],
            append_images=images[1:])
    finally:
        shutil.rmtree(tmpdir, ignore_errors=True)

if __name__ == '__main__':
    if len(sys.argv) != 3:
        print(f"Usage: {sys.argv[0]} <input.svg> <output.ico>", file=sys.stderr)
        sys.exit(1)
    generate_icon(sys.argv[1], sys.argv[2])
    print(f"OK {sys.argv[2]}")
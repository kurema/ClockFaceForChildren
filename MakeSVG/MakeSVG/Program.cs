using System;
using System.Drawing;
using Svg;

namespace MakeSVG
{
    class Program
    {
        static void Main(string[] args)
        {
            (float sin, float cos) GetSinCos(float i, float max)
            {
                return (MathF.Sin(i / max * 2.0f * MathF.PI),
                    -MathF.Cos(i / max * 2.0f * MathF.PI)
                    );
            }

            float canvsSize = 400;

            var doc = new SvgDocument { Width = canvsSize, Height = canvsSize };
            doc.ViewBox = new SvgViewBox(-canvsSize / 2, -canvsSize / 2, canvsSize, canvsSize);

            string font = "UD デジタル 教科書体 NP-B";

            {
                float rad = 100;
                float fontSize = 18;
                {
                    var group = new SvgGroup()
                    {
                        ID = "HourText"
                    };
                    doc.Children.Add(group);
                    for (int i = 1; i <= 12; i++)
                    {
                        var rgb = HsvToRgb((i + 11) % 12 / 12.0f, 1, 0.9f);

                        var text = new SvgText(i + "")
                        {
                            FontSize = fontSize,
                            FontWeight = SvgFontWeight.Bold,
                            TextAnchor = SvgTextAnchor.Middle,
                            FontFamily = font,
                            Fill = new SvgColourServer(Color.FromArgb(rgb.r, rgb.g, rgb.b)),
                            X = new SvgUnitCollection()
                            {
                                new SvgUnit(rad*MathF.Sin((i+0.5f)/6.0f*MathF.PI)),
                            },
                            Y = new SvgUnitCollection()
                            {
                                new SvgUnit(-rad*MathF.Cos((i+0.5f)/6.0f*MathF.PI)),
                            }
                        };
                        text.CustomAttributes.Add("dominant-baseline", "middle");

                        group.Children.Add(text);
                    }

                }
                {
                    float radW = 40 / 2.0f;
                    var group = new SvgGroup()
                    {
                        ID = "HourLine"
                    };
                    doc.Children.Add(group);

                    var group2 = new SvgGroup()
                    {
                        ID = "CenterCircle"
                    };
                    doc.Children.Add(group2);

                    var circle = new SvgCircle()
                    {
                        Radius = rad + radW,
                        Fill = new SvgColourServer(Color.Transparent),
                        Stroke = new SvgColourServer(Color.Black),
                        StrokeWidth = 0.5f,
                    };
                    group.Children.Add(circle);

                    for (int i = 0; i < 12; i++)
                    {
                        var coord = GetSinCos(i, 12);
                        var coord2 = GetSinCos(i + 1, 12);

                        var POut = new PointF(coord.sin * (rad + radW), coord.cos * (rad + radW));
                        var PIn = new PointF(coord.sin * (rad - radW), coord.cos * (rad - radW));
                        var PInNext = new PointF(coord2.sin * (rad - radW), coord2.cos * (rad - radW));

                        var line = new SvgLine()
                        {
                            StartX = POut.X,
                            EndX = PIn.X,
                            StartY = POut.Y,
                            EndY = PIn.Y,
                            Stroke = new SvgColourServer(Color.Black),
                            StrokeWidth = 0.5f,
                        };
                        group.Children.Add(line);

                        var rgb = HsvToRgb((i + 11) % 12 / 12.0f, 0.7f, 1.0f);

                        var path = new SvgPath()
                        {
                            StrokeWidth = 1,
                            Stroke = new SvgColourServer(Color.Transparent),
                            Fill = new SvgColourServer(Color.FromArgb(rgb.r, rgb.g, rgb.b)),
                            PathData = new Svg.Pathing.SvgPathSegmentList(),
                        };
                        path.PathData.Add(new Svg.Pathing.SvgMoveToSegment(PIn));
                        path.PathData.Add(new Svg.Pathing.SvgArcSegment(
                            PIn
                            , rad - radW, rad - radW
                            , i / 12.0f - 0.5f, Svg.Pathing.SvgArcSize.Small, Svg.Pathing.SvgArcSweep.Positive
                            , PInNext));
                        path.PathData.Add(new Svg.Pathing.SvgLineSegment(PInNext, new PointF(0, 0)));
                        path.PathData.Add(new Svg.Pathing.SvgLineSegment(new PointF(0, 0), PIn));
                        group2.Children.Add(path);
                    }
                }
            }
            {
                float rad = 145;
                float fonsSize = 40;

                {
                    var group = new SvgGroup()
                    {
                        ID = "HourTextBig"
                    };
                    doc.Children.Add(group);
                    for (int i = 1; i <= 12; i++)
                    {
                        var text = new SvgText(i.ToString())
                        {
                            FontSize = fonsSize,
                            FontWeight = SvgFontWeight.W900,
                            TextAnchor = SvgTextAnchor.Middle,
                            Fill = new SvgColourServer(Color.FromArgb(255, 0, 89)),
                            FontFamily = font,
                            X = new SvgUnitCollection()
                            {
                                new SvgUnit(rad*MathF.Sin((i+0f)/6.0f*MathF.PI)),
                            },
                            Y = new SvgUnitCollection()
                            {
                                new SvgUnit(-rad*MathF.Cos((i+0f)/6.0f*MathF.PI)),
                            }
                        };
                        text.CustomAttributes.Add("dominant-baseline", "middle");
                        group.Children.Add(text);
                    }

                }
            }

            {
                var group = new SvgGroup()
                {
                    ID = "SecLine",
                };
                doc.Children.Add(group);

                var group2 = new SvgGroup()
                {
                    ID = "SecText",
                };
                doc.Children.Add(group2);


                var red = new SvgColourServer(Color.FromArgb(30, 41, 210));

                float rad = 175;
                float radS = 5;
                float fontSize = 10f;
                float radW = 15f;

                {
                    var circle = new SvgCircle()
                    {
                        Radius = rad,
                        Fill = new SvgColourServer(Color.Transparent),
                        Stroke = red,
                        StrokeWidth = 1f,
                    };
                    group.Children.Add(circle);
                }
                {
                    var circle = new SvgCircle()
                    {
                        Radius = rad + radW,
                        Fill = new SvgColourServer(Color.Transparent),
                        Stroke = red,
                        StrokeWidth = 1f,
                    };
                    group.Children.Add(circle);
                }

                for (int i = 0; i < 60; i++)
                {
                    bool isx5 = i % 5 == 0;

                    if (isx5)
                    {
                        float angleT = 0.4f;
                        var coord1 = GetSinCos(i - angleT, 60);
                        var coord2 = GetSinCos(i + angleT, 60);

                        var P11 = new PointF(coord1.sin * rad, coord1.cos * rad);
                        var P12 = new PointF(coord2.sin * rad, coord2.cos * rad);
                        var P21 = new PointF(coord1.sin * (rad + radW), coord1.cos * (rad + radW));
                        var P22 = new PointF(coord2.sin * (rad + radW), coord2.cos * (rad + radW));

                        var path = new SvgPath()
                        {
                            StrokeWidth = 1,
                            Stroke = new SvgColourServer(Color.Transparent),
                            Fill = red,
                            PathData = new Svg.Pathing.SvgPathSegmentList(),
                        };

                        path.PathData.Add(new Svg.Pathing.SvgMoveToSegment(P11));
                        path.PathData.Add(new Svg.Pathing.SvgArcSegment(
                            P11
                            , rad, rad
                            , (i - angleT) / 60.0f - 0.5f, Svg.Pathing.SvgArcSize.Small, Svg.Pathing.SvgArcSweep.Positive
                            , P12));
                        path.PathData.Add(new Svg.Pathing.SvgLineSegment(P12, P22));
                        path.PathData.Add(new Svg.Pathing.SvgArcSegment(
                            P22
                            , rad, rad
                            , (i - angleT) / 60.0f + 0.5f, Svg.Pathing.SvgArcSize.Small, Svg.Pathing.SvgArcSweep.Positive
                            , P21));
                        path.PathData.Add(new Svg.Pathing.SvgLineSegment(P21, P11));

                        group.Children.Add(path);
                    }


                    (float sin, float cos) = GetSinCos(i, 60);

                    var line = new SvgLine()
                    {
                        Stroke = red,
                        StrokeWidth = isx5 ? 3.0f : 1.0f,
                        StrokeLineCap = SvgStrokeLineCap.Round,
                        StartX = sin * (rad - radS),
                        EndX = sin * rad,
                        StartY = cos * (rad - radS),
                        EndY = cos * rad,
                    };
                    group.Children.Add(line);

                    var text = new SvgText(i.ToString())
                    {
                        FontSize = fontSize,
                        FontWeight = SvgFontWeight.Bold,
                        TextAnchor = SvgTextAnchor.Middle,
                        Fill = isx5 ? new SvgColourServer(Color.White) : red,
                        X = new SvgUnitCollection()
                            {
                                new SvgUnit((rad+(radW)/2.0f)*sin),
                            },
                        Y = new SvgUnitCollection()
                            {
                                new SvgUnit((rad+(radW)/2.0f)*cos),
                            }
                    };
                    text.CustomAttributes.Add("dominant-baseline", "middle");
                    group2.Children.Add(text);
                }
            }
            doc.Write("out.svg");
        }
        public static (int r, int g, int b) HsvToRgb(float h, float s, float v)
        {
            //https://ja.wikipedia.org/wiki/HSV%E8%89%B2%E7%A9%BA%E9%96%93#HSV%E3%81%8B%E3%82%89RGB%E3%81%B8%E3%81%AE%E5%A4%89%E6%8F%9B
            float r = v;
            float g = v;
            float b = v;
            if (s <= 0.0f) return ((int)(r * 255), (int)(g * 255), (int)(b * 255));
            h *= 6.0f;
            int i = (int)h;
            float f = h - (float)i;
            switch (i)
            {
                default:
                case 0:
                    g *= 1 - s * (1 - f);
                    b *= 1 - s;
                    break;
                case 1:
                    r *= 1 - s * f;
                    b *= 1 - s;
                    break;
                case 2:
                    r *= 1 - s;
                    b *= 1 - s * (1 - f);
                    break;
                case 3:
                    r *= 1 - s;
                    g *= 1 - s * f;
                    break;
                case 4:
                    r *= 1 - s * (1 - f);
                    g *= 1 - s;
                    break;
                case 5:
                    g *= 1 - s;
                    b *= 1 - s * f;
                    break;
            }
            return ((int)(r * 255), (int)(g * 255), (int)(b * 255));
        }
    }
}

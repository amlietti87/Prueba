using GeoJSON.Net.Geometry;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Interfaces.Services;
using ROSBUS.Admin.Domain.Url;
using SharpKml.Base;
using SharpKml.Dom;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.Caching;


namespace ROSBUS.Admin.Domain.Maps
{
    public class SharpKMLHelper : ISharpKMLHelper
    {
        private readonly IAppUrlService _appUrlService;

        public SharpKMLHelper(IAppUrlService appUrlService)
        {
            _appUrlService = appUrlService;
        }

        public Placemark CreatePlaceMark(string title, SharpKml.Dom.Geometry geometry)
        {
            Placemark placeMark = new Placemark();
            placeMark.Name = title;
            placeMark.Geometry = geometry;

            return placeMark;
        }


        public Style CreatePlacemarkPinche(string name, bool highlight, string url)
        {
            Style styleNode = new Style();
            // Add Line Style
            IconStyle iconStyle = new IconStyle();
            if (!highlight)
            {
                styleNode.Id = String.Format("{0}-normal", name);
                iconStyle.Icon = new IconStyle.IconLink(new Uri(url));
            }
            else
            {
                styleNode.Id = String.Format("{0}-highlight", name);
                iconStyle.Icon = new IconStyle.IconLink(new Uri(url));
            }
            iconStyle.Hotspot = new Hotspot() { X = 20, Y = 2, XUnits = Unit.Pixel, YUnits = Unit.Pixel };
            iconStyle.Scale = 1.1;

            styleNode.Icon = iconStyle;

            return styleNode;
        }

        public Style CreatePlacemarkStartFinish(string name, bool highlight, string url)
        {
            Style styleNode = new Style();
            // Add Line Style
            IconStyle iconStyle = new IconStyle();

            if (!highlight)
            {

                styleNode.Id = String.Format("{0}-normal", name);
                iconStyle.Icon = new IconStyle.IconLink(new Uri(url));
            }
            else
            {
                styleNode.Id = String.Format("{0}-highlight", name);
                iconStyle.Icon = new IconStyle.IconLink(new Uri(url));
            }
            //iconStyle.Hotspot = new Hotspot() { X = 32, Y = 64, XUnits = Unit.Pixel, YUnits = Unit.InsetPixel };

            LabelStyle labelStyle = new LabelStyle();
            labelStyle.Scale = 0;

            styleNode.Icon = iconStyle;
            styleNode.Label = labelStyle;
            return styleNode;
        }

        public Style CreatePlacemarkLineStyle(string name, bool highlight, string Color)
        {
            Style styleNode = new Style();
            // Add Line Style
            LineStyle lineStyle = new LineStyle();

            string hexcolor;

            if (String.IsNullOrWhiteSpace(Color) || Color.Length != 6)
            {
                hexcolor = "1446B4";
            }
            else
            {
                hexcolor = Color;
            }

            if (!highlight)
            {
                styleNode.Id = String.Format("{0}-normal", name);
                lineStyle.Color = FromHex(hexcolor);
                lineStyle.Width = 5;
            }
            else
            {
                styleNode.Id = String.Format("{0}-highlight", name);
                lineStyle.Color = FromHex(hexcolor);
                lineStyle.Width = 7.5;
            }
            styleNode.Line = lineStyle;
            return styleNode;
        }

        public Color32 FromHex(string hex)
        {
            if (hex.StartsWith("#"))
                hex = hex.Substring(1);

            if (hex.Length != 6) throw new Exception("Color not valid");

            var color = Color.FromArgb(
                int.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber),
                int.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber),
                int.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber));

            return new Color32(color.A, color.B, color.G, color.R);
        }

        public StyleSelector CreatePlacemarkStyleMap(String name, Style normalStyle, Style highlightStyle)
        {
            // Set up style map
            StyleMapCollection styleMapCollection = new StyleMapCollection();
            styleMapCollection.Id = String.Format("{0}-stylemap", name);
            // Create the normal line pair
            Pair normalPair = new Pair();
            normalPair.StyleUrl = new Uri(String.Format("#{0}", normalStyle.Id), UriKind.Relative);
            normalPair.State = StyleState.Normal;
            // Create the highlight line pair
            Pair highlightPair = new Pair();
            highlightPair.StyleUrl = new Uri(String.Format("#{0}", highlightStyle.Id), UriKind.Relative);
            highlightPair.State = StyleState.Highlight;
            // Attach both pairs to the map
            styleMapCollection.Add(normalPair);
            styleMapCollection.Add(highlightPair);

            return styleMapCollection;
        }

        public async Task AddStartAndFinishIconsToDocument(PuntoWithCoordenada primerpunto, PuntoWithCoordenada segundopunto, Folder document, StyleSelector styleSelectorStart, StyleSelector styleSelectorFinish)
        {

            SharpKml.Dom.Point point1 = new SharpKml.Dom.Point();
            point1.Coordinate = new Vector(primerpunto.Lat, primerpunto.Long);

            Placemark placemark1 = this.CreatePlaceMark(primerpunto.CoordenadaDescription, point1);
            placemark1.StyleUrl = new Uri(String.Format("#{0}", styleSelectorStart.Id), UriKind.Relative);
            document.AddFeature(placemark1);




            SharpKml.Dom.Point point2 = new SharpKml.Dom.Point();
            point2.Coordinate = new Vector(segundopunto.Lat, segundopunto.Long);

            Placemark placemark2 = this.CreatePlaceMark(segundopunto.CoordenadaDescription, point2);
            placemark2.StyleUrl = new Uri(String.Format("#{0}", styleSelectorFinish.Id), UriKind.Relative);
            document.AddFeature(placemark2);

        }

        public CoordinateCollection CreateCoordinateCollection(List<Position> points)
        {


            CoordinateCollection coordinates = new CoordinateCollection();
            foreach (var item in points)
            {
                coordinates.Add(new Vector(item.Latitude, item.Longitude));
            }


            return coordinates;
        }

    }

    public class PuntoWithCoordenada
    {
        public double Lat { get; set; }
        public double Long { get; set; }
        public string CoordenadaDescription { get; set; }
    }
}

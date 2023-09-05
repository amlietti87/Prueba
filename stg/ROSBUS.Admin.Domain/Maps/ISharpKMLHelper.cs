using GeoJSON.Net.Geometry;
using SharpKml.Base;
using SharpKml.Dom;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace ROSBUS.Admin.Domain.Maps
{
    public interface ISharpKMLHelper
    {
        Placemark CreatePlaceMark(string title, SharpKml.Dom.Geometry geometry);
        Style CreatePlacemarkPinche(string name, bool highlight, string url);

        Style CreatePlacemarkStartFinish(string name, bool highlight, string url);

        Style CreatePlacemarkLineStyle(string name, bool highlight, string Color);

        Color32 FromHex(string hex);

        StyleSelector CreatePlacemarkStyleMap(String name, Style normalStyle, Style highlightStyle);

        Task AddStartAndFinishIconsToDocument(PuntoWithCoordenada primerpunto, PuntoWithCoordenada segundopunto, Folder document, StyleSelector styleSelectorStart, StyleSelector styleSelectorFinish);
        CoordinateCollection CreateCoordinateCollection(List<Position> points);
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CherwellTest.Utilites;

namespace CherwellTest.Controllers
{
    public class LayoutController : ApiController
    {
        // GET api/values?RowLetter=A&ColumnNumber=5
        public List<Point> Get(char RowLetter, int ColumnNumber)
        {
            var layout = new TriangleLayout(6, 12);
            var triangleVerticies = layout.GetVerticies(new TriangleLocation(RowLetter, ColumnNumber));
            return triangleVerticies.Verticies;
        }

        //GET api/layout?V1=30,50&V2=30,60&V3=40,50
        public LayoutLocation Get(Point V1, Point V2, Point V3)
        {
            var layout = new TriangleLayout(6, 12);
            var triangleLocation = layout.GetTriangleLocation(new TriangleVerticies(V1, V2, V3));
            return triangleLocation;
        }

        //// POST api/values
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/values/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/values/5
        //public void Delete(int id)
        //{
        //}
    }
}

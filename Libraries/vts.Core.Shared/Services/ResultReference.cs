using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Text;

namespace vts.Shared.Services
{
    public  class ResultReference
    {
        private static readonly Random Random = new Random((int)DateTime.Now.Ticks);
        private string RandomString(int size)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < size; i++)
            {
                var ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * Random.NextDouble() + 65)));
                builder.Append(ch);
            }
            return builder.ToString();
        }
        public string Generate(string pollingCentre, string name)
        {
            var date = DateTime.Now;
            string reference = RandomString(5) + "_" + date + "_" + name + "_" + pollingCentre;
            return reference;
        }
    }
}

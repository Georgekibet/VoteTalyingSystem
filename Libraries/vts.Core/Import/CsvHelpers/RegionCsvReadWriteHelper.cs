using System;
using System.Collections.Generic;
using System.IO;
using CsvHelper;
using vts.Shared.Entities.Master;

namespace vts.Core.Import.CsvHelpers
{
    public class RegionCsvReadWriteHelper: ICsvReadWriteHelper<Region>
    {
        public RegionCsvReadWriteHelper()
        {
        }

        public CsvReadResults<Region> ReadCsv(string path)
        {

            var resp = new CsvReadResults<Region>();
            try
            {

                var streamReader = new StreamReader(path);
                var csv = new CsvReader(streamReader);
                csv.Configuration.HasHeaderRecord = false;
                csv.Configuration.SkipEmptyRecords = true;
                csv.Configuration.Delimiter = ",";
                csv.Configuration.IgnoreReadingExceptions = false;
                csv.Configuration.TrimFields = true;


                List<Region> importList = new List<Region>();
                List<string> ignoredList = new List<string>();
                var count=0;
                while (csv.Read())
                {
                    string name;
                    string code;

                    csv.TryGetField<string>(0, out name);
                    csv.TryGetField<string>(1, out code);

                    if (!string.IsNullOrEmpty(name) &&  !string.IsNullOrEmpty(code))
                    {
                        var record = new Region()
                        {
                            Name = name,
                            Code = code
                        };
                        importList.Add(record);
                    }
                    else
                    {
                        ignoredList.Add(csv.GetField(0) + " " + csv.GetField(1));
                    }
                    count++;
                }
                streamReader.Close();


                resp.Imported = importList;
                resp.Ignored = ignoredList;
                resp.TotalRecords = count;
            }
            catch (Exception ex)
            {

            }

            return resp;
        }

    }
}
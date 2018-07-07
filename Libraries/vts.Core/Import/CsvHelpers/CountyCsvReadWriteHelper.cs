using CsvHelper;
using System;
using System.Collections.Generic;
using System.IO;
using vts.Core.Import.models;
using vts.Shared.Repository;

namespace vts.Core.Import.CsvHelpers
{
    public class CountyCsvReadWriteHelper : ICsvReadWriteHelper<CountyImportModel>
    {
        private IRegionRepository _regionRepository;

        public CountyCsvReadWriteHelper(IRegionRepository regionRepository)
        {
            _regionRepository = regionRepository;
        }

        public CsvReadResults<CountyImportModel> ReadCsv(string path)
        {
            var resp = new CsvReadResults<CountyImportModel>();
            try
            {
                var streamReader = new StreamReader(path);
                var csv = new CsvReader(streamReader);
                csv.Configuration.HasHeaderRecord = false;
                csv.Configuration.SkipEmptyRecords = true;
                csv.Configuration.Delimiter = ",";
                csv.Configuration.IgnoreReadingExceptions = false;
                csv.Configuration.TrimFields = true;

                List<CountyImportModel> importList = new List<CountyImportModel>();
                List<string> ignoredList = new List<string>();
                var count = 0;
                while (csv.Read())
                {
                    string name;
                    string code;
                    string region;
                    string registeredVoters;

                    csv.TryGetField<string>(0, out name);
                    csv.TryGetField<string>(1, out code);
                    csv.TryGetField<string>(2, out region);

                    if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(code))
                    {
                        var record = new CountyImportModel()
                        {
                            Name = name,
                            Code = code,
                            Region = region
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
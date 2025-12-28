using System.Data;
using System.Reflection;
using QL_DienTuGiaDung.Models;

namespace QL_DienTuGiaDung.Helpers
{
    public static class DataHelper
    {
        public static List<T> MapToList<T>(List<Dictionary<string, object?>> data)
        where T : new()
        {
            var result = new List<T>();
            var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var row in data)
            {
                var obj = new T();

                foreach (var prop in props)
                {
                    if (!row.ContainsKey(prop.Name))
                        continue;

                    var value = row[prop.Name];

                    if (value == null || value == DBNull.Value)
                    {
                        prop.SetValue(obj, null);
                        continue;
                    }

                    var targetType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;

                    prop.SetValue(obj, Convert.ChangeType(value, targetType));
                }

                result.Add(obj);
            }

            return result;
        }

        public static DataTable MapToDataTable(List<SanPhamDatHang> list)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("MaSP", typeof(int));
            dt.Columns.Add("SoLuongDat", typeof(int));

            foreach (var item in list)
            {
                dt.Rows.Add(
                    item.MaSP,
                    item.SoLuongDat
                );
            }
            return dt;
        }
    }
}

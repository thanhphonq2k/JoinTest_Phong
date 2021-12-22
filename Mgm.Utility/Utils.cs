using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Configuration;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace Mgm.Utility
{
    public class Utils
    {
        public string MD5Hash(string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            //compute hash from the bytes of text  
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));

            //get hash result after compute it  
            byte[] result = md5.Hash;

            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                //change it into 2 hexadecimal digits  
                //for each byte  
                strBuilder.Append(result[i].ToString("x2"));
            }

            return strBuilder.ToString();
        }

        public decimal KVA(string vsKVA)
        {
            // Get the first KVA from the string if it has a primary / secondary in it.
            // Ex:    1600/2300 returns 1600.
            //        112.5 returns 112.5.
            decimal curKVA = 0;
            int iLen;
            char sChar;
            string sKVA = "";

            vsKVA = vsKVA.Trim();
            iLen = vsKVA.Length;

            for (var k = 0; k < iLen; k++)
            {
                sChar = vsKVA[k];
                if (char.IsDigit(sChar) || sChar == '.')
                {
                    sKVA += sChar;
                }
                else
                {
                    break;
                }
            }

            if (!string.IsNullOrEmpty(sKVA))
            {
                curKVA = decimal.Parse(sKVA);
            }

            return curKVA;
        }

        public int NumberFirstInString(string vvarString)
        {
            // Return the first integer found in a string.
            // Used when Phase returns "1 c/o-1" instead of "1", for example.
            int j;
            string sVal;

            sVal = "";
            vvarString = !string.IsNullOrEmpty(vvarString) ? vvarString.Trim() : "";

            char vvarChar = ' ';

            switch (vvarString)
            {
                case "":
                case "Three":
                    vvarString = "3";
                    vvarChar = '3';
                    break;
                case "Single":
                    vvarString = "1";
                    vvarChar = '1';
                    break;
            }

            if (char.IsDigit(vvarChar))
            {
                j = Convert.ToInt32(vvarString);
                return j;
            }

            var arrChar = vvarString.ToCharArray();
            char c;
            for (int i = 0; i < arrChar.Length; i++)
            {
                c = arrChar[i];
                if (char.IsDigit(c) && c != '-')
                    sVal += c;
                else
                    break;
            }

            j = Convert.ToInt32(sVal);

            return j;
        }

        /*
        public string UQ(string vvarIn)
        {
            vvarIn = vvarIn.Trim();

            // Replace internal quote mark with apostrophe.
            if ((vvarIn.IndexOf("\"") + 1) > 0)
            {
                vvarIn = vvarIn.Replace("\"", "'");
            }

            if ((vvarIn.IndexOf("'") + 1) > 0)
            {
                vvarIn = vvarIn.Replace("'", "''");
            }
            
            return "'" + (vvarIn + "'");
        }
        */

        public Cell CreateTextCell(int columnIndex, int rowIndex, string cellValue, string format = "")
        {
            Cell cell = new Cell();
            cell.CellReference = GetExcelColumnName(columnIndex) + rowIndex;
            int resInt;
            double resDouble;
            DateTime resDate;

            if (int.TryParse(cellValue, out resInt))
            {
                CellValue v = new CellValue();
                v.Text = resInt.ToString();
                cell.AppendChild(v);
            }
            else if (double.TryParse(cellValue, out resDouble))
            {
                CellValue v = new CellValue();
                v.Text = resDouble.ToString();
                cell.AppendChild(v);
            }
            else if (DateTime.TryParse(cellValue, out resDate))
            {
                cell.DataType = CellValues.InlineString;
                InlineString inlineString = new InlineString();
                Text t = new Text();

                if (string.IsNullOrEmpty(format))
                {
                    t.Text = resDate.ToString("MM/dd/yyyy");
                }
                else
                {
                    t.Text = resDate.ToString(format);
                }

                inlineString.AppendChild(t);
                cell.AppendChild(inlineString);
            }
            else
            {
                cell.DataType = CellValues.InlineString;
                InlineString inlineString = new InlineString();
                Text t = new Text();

                t.Text = cellValue == null ? "" : cellValue.ToString();
                inlineString.AppendChild(t);
                cell.AppendChild(inlineString);
            }

            return cell;
        }

        public string GetExcelColumnName(int columnIndex)
        {
            int dividend = columnIndex;
            string columnName = String.Empty;
            int modifier;

            while (dividend > 0)
            {
                modifier = (dividend - 1) % 26;
                columnName = Convert.ToChar(65 + modifier).ToString() + columnName;
                dividend = (int)((dividend - modifier) / 26);
            }

            return columnName;
        }

        public void WriteLog(Exception ex, string pObjErr, string logFile)
        {
            string confLogDir = ConfigurationManager.AppSettings["JobLogFolder"] + "\\";

            string sWebsitePath = confLogDir;
            string path = sWebsitePath + logFile;
            if (!CheckIfFileIsBeingUsed(path))
            {
                FileInfo oFileInfo = new FileInfo(path);
                DirectoryInfo oDirInfo = new DirectoryInfo(oFileInfo.DirectoryName);

                if (!oDirInfo.Exists)
                {
                    //oDirInfo.Create();
                }

                FileStream fs = new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.Write);
                using (StreamWriter w = new StreamWriter(fs))
                {
                    if (null == ex)
                    {
                        w.WriteLine(
                            "--------------------------------------------------------------------------------------");
                        w.WriteLine("<Log Entry> : {0} {1}", DateTime.Now.ToLongTimeString(),
                                    DateTime.Now.ToLongDateString());
                        w.WriteLine("<Object> : " + pObjErr);
                        w.WriteLine(
                            "--------------------------------------------------------------------------------------");
                        w.Flush();
                    }
                    else if (!(ex is ThreadAbortException))
                    {
                        w.WriteLine(
                            "--------------------------------------------------------------------------------------");
                        w.WriteLine("<Log Entry> : {0} {1}", DateTime.Now.ToLongTimeString(),
                                    DateTime.Now.ToLongDateString());
                        w.WriteLine("<Message> : " + ex.Message);
                        w.WriteLine("<StackTrace> : " + ex.StackTrace);
                        w.WriteLine("<Source> : " + ex.Source);
                        w.WriteLine("<TargetSite> : " + ex.TargetSite.ToString());
                        w.WriteLine("<Object> : " + pObjErr);
                        w.WriteLine(
                            "--------------------------------------------------------------------------------------");
                        w.Flush();
                    }
                    w.Close();
                }
                fs.Close();
            }
        }

        public bool CheckIfFileIsBeingUsed(string fileName)
        {
            try
            {
                FileStream fs;
                fs = File.Open(fileName, FileMode.OpenOrCreate, FileAccess.Read, FileShare.None);
                fs.Close();
            }
            catch
            {
                return true;
            }
            return false;
        }
    }
}

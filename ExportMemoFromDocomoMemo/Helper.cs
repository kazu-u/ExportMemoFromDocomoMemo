using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExportMemoFromDocomoMemo
{
    /**
     * チェック用クラス
     */
    internal class Helper
    {
        /**
         * ファイル存在チェック
         * @param filePath ファイルパス
         */
        static public bool CheckExistsFile(string filePath)
        {
            if (String.IsNullOrEmpty(filePath)) return false;
            FileInfo fileInfo = new FileInfo(filePath);
            return fileInfo.Exists;
        }

        /**
         * ディレクトリ存在チェック
         * @param filePath ファイルパス
         */
        static public bool CheckExistsDirecotry(string filePath)
        {
            String directoryPath = GetFolderPath(filePath);
            DirectoryInfo directoryInfo = new DirectoryInfo(directoryPath);
            return directoryInfo.Exists;
        }

        /**
         * 親ディレクトリ返却用
         */
        static  public String GetFolderPath(string filePath)
        {
            if (String.IsNullOrEmpty(filePath)) return String.Empty;
            FileInfo fileInfo = new FileInfo(filePath);
            return String.IsNullOrEmpty(fileInfo.DirectoryName)?String.Empty:fileInfo.DirectoryName;
        }

        /**
         * 読み込みレコードから、最後尾の"="を削除する
         */
        static public String RemoveLastEqualChar(String input)
        {
            if (input.Length > 0 && input.Substring(input.Length - 1, 1).Equals("="))
            {
                return input[..^1];
            }
            return input;
        }

        /*
         * QuotedPirtable デコード処理　（UTF-8を前提）
         */
        static public String DecodeQuotedPritable(string input)
        {
            var bytes = new List<Byte>();

            // 文字位置
            var index = 0;

            while (index < input.Length)
            {
                var c = input[index];

                if (c == '=')
                {
                    // =の後ろ2文字抽出し、バイト変換し、バイナリ配列に格納
                    var octetStr = input.Substring(index + 1, 2);
                    var octet = Convert.ToByte(octetStr, 16);
                    bytes.Add(octet);
                    index += 2;
                }
                else
                {
                    bytes.Add((byte)c);
                }

                index++;
            }

            // 作成したバイト配列で、UTF8でエンコードする
            Encoding encoding = new UTF8Encoding();
            var result = encoding.GetString(bytes.ToArray());
            return result;
        }

    }
}

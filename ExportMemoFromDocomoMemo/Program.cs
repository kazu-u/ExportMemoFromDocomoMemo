using System.Text;

namespace ExportMemoFromDocomoMemo
{
    internal class Program
    {
        // 出力ファイル名（未指定時）
        const String OUTPUT_FILE_NAME = "exportMemo.txt";
        // 開始
        private const String BEGIN_VEVENT = "BEGIN:VEVENT";
        // 終了
        private const String END_VEVENT = "END:VEVENT";
        // サマリ
        private const String SUMMARY = "SUMMARY;CHARSET=UTF-8;ENCODING=QUOTED-PRINTABLE:";
        // 詳細
        private const String DESCRIPTION = "DESCRIPTION;CHARSET=UTF-8;ENCODING=QUOTED-PRINTABLE:";
        // カテゴリ
        private const String CATEGORIES = "CATEGORIES:";
        // ロケーション
        private const String LOCATION = "LOCATION:";
        // メモ
        private const String DOCOMO_MEMO = "X-DCM-MEMO";

        static void Main(string[] args)
        {
            // 引数チェック
            (bool argsCheckResult, String inputFilePath, String outputFilePath) = CheckArguments(args);
            if (!argsCheckResult) return;

            // ファイル読み込み処理
            try
            {
                List<DocomoMemoType>? docomoMemoTypes = ReadVCSFile(inputFilePath);
                if(docomoMemoTypes.Count == 0)
                {
                    Console.WriteLine("出力するメモ情報が存在しませんでした。");
                    return;
                }

                Output(outputFilePath, docomoMemoTypes);

                Console.WriteLine("出力が正常に終了しました。");
                Console.WriteLine("出力ファイルパス：" + outputFilePath);

            }
            catch (Exception )
            {
                Console.WriteLine("ドコモメモcvsファイルからのメモ出力に失敗しました。");
                return;
            }

        }

        /**
         * 引数チェック
         */
        static (bool,string,string) CheckArguments(string[] args)
        {
            string inputFilePath = String.Empty;

            bool isParamFormatError = false;

            if (args.Length == 2)
            {
                if (args[0].Equals("-i"))
                {
                    // -iオプションの場合、
                    inputFilePath = args[1];
                    isParamFormatError = true;
                }
            }

            if (!isParamFormatError)
            {
                Console.WriteLine("パラメータ不正");
                Console.WriteLine("Usage: ExportMemoFromDocomoMemo -i <メモファイルパス(.vcs)>");
                Console.WriteLine("※-o パラメータは任意です。 ");
                Console.WriteLine("例: ExportMemoFromDocomoMemo -i \"C:\\Users\\ユーザ名\\download\\20240912194721.vcs\"");
                return (false, string.Empty, string.Empty);
            }

            // チェック処理
            if (!Helper.CheckExistsFile(inputFilePath))
            {
                // ファイルが存在しない場合
                Console.WriteLine("指定されたファイルが存在しません。(パス：" + inputFilePath + ")");
                return (false,inputFilePath,string.Empty);
            }

            // 出力ファイルパスが指定されていない場合、入力ファイルと同じディレクトリにファイルを出力する
            String outputFilePath = Helper.GetFolderPath(inputFilePath) + "/" + OUTPUT_FILE_NAME;

            return (true,inputFilePath,outputFilePath);
        }

        /**
         * VCSファイルの読み込み
         */
        static List<DocomoMemoType> ReadVCSFile(string filePath)
        {

            List<DocomoMemoType> documentMemoTypes = new();
            DocomoMemoType memoType = new()
            {
                summary = new StringBuilder(),
                description = new StringBuilder(),
                categories = string.Empty
            };

            try
            {
                // 読込用
                using (StreamReader sr = new StreamReader(filePath))
                {
                    // 1行ずつ読み込み
                    while (!sr.EndOfStream)
                    {

                        String? line = sr.ReadLine();
                        if (String.IsNullOrEmpty(line)) break ;

                        // データ項目ごとに処理を分ける
                        if (line.StartsWith(BEGIN_VEVENT))
                        {
                            // 開始行
                            memoType = new DocomoMemoType
                            {
                                summary = new StringBuilder(),
                                description = new StringBuilder(),
                                categories = string.Empty
                            };

                        }
                        else if(line.StartsWith(SUMMARY))
                        {
                            // サマリ
                            memoType.summary.Append(Helper.RemoveLastEqualChar(line.Replace(SUMMARY, String.Empty)));
                            while (true)
                            {
                                String? lineTmp = sr.ReadLine();
                                if (lineTmp == null || lineTmp.StartsWith(LOCATION)) break;
                                memoType.summary.Append(Helper.RemoveLastEqualChar(lineTmp));
                            }
                        }
                        else if (line.StartsWith(DESCRIPTION))
                        {
                            // 詳細行
                            memoType.description.Append(Helper.RemoveLastEqualChar(line.Replace(DESCRIPTION, String.Empty)));
                            while (true)
                            {
                                String? lineTmp = sr.ReadLine();
                                if (lineTmp == null) break;
                                if (lineTmp.StartsWith(CATEGORIES))
                                {
                                    memoType.categories = lineTmp.Replace(CATEGORIES, String.Empty);
                                    break;
                                }
                                memoType.description.Append(Helper.RemoveLastEqualChar(lineTmp));
                            }
                        }
                        else if (line.StartsWith(END_VEVENT))
                        {
                            // 終了行 メモ以外の場合は、読み捨て
                            if (memoType.categories.Equals(DOCOMO_MEMO))
                            {
                                // 追加
                                documentMemoTypes.Add(memoType);
                            }
                        }
                    }
                }

                return documentMemoTypes;
            }
            catch (Exception)
            {
                throw ;
            }


        }

        /**
         * VCSファイルの読み込み
         */
        static void Output(string filePath, List<DocomoMemoType> memoTypes)
        {
            try
            {
                // 読込用
                using (StreamWriter sw = new StreamWriter(filePath))
                {
                    int count = 1;
                    memoTypes.ForEach(memoType =>
                    {
                        sw.WriteLine("■メモ[" + count + "]");
                        if (memoType.summary.Length > 0)
                        {
                            sw.WriteLine("【　内容　】");
                            sw.WriteLine(Helper.DecodeQuotedPritable(memoType.summary.ToString()));
                            sw.WriteLine(string.Empty);
                        }
                        if (memoType.description.Length > 0)
                        {
                            sw.WriteLine("【　詳細　】");
                            sw.WriteLine(Helper.DecodeQuotedPritable(memoType.description.ToString()));
                            sw.WriteLine(string.Empty);
                        }
                        count++;
                    });
                }

                return ;
            }
            catch (Exception)
            {
                throw;
            }


        }


    }
}
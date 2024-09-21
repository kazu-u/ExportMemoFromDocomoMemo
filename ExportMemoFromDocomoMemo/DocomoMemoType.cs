using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExportMemoFromDocomoMemo
{
    /**
     * ドコモのメモ情報
     */
    internal struct DocomoMemoType
    {
        /** 内容*/
        public StringBuilder summary;

        /** 詳細 */
        public StringBuilder description;
        /** カテゴリ */
        public string categories;
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace vrmBluePrinter
{
    /// <summary>
    /// モジュールリスト
    /// </summary>
    public class moduleList
    {
        /// <summary>
        /// モジュールNo
        /// </summary>
        public int moduleNo { get; set; }

        /// <summary>
        /// モジュール名
        /// </summary>
        public string moduleTitle { get; set; }

        /// <summary>
        /// モジュールクラス名
        /// </summary>
        public string moduleClassTitle { get; set; }

        /// <summary>
        /// モジュールクラス
        /// </summary>
        public object moduleClass {  get; set; }

        public moduleList()
        {
            moduleNo = -1;
            moduleTitle = string.Empty;
            moduleClassTitle = string.Empty;
            moduleClass = null;
            
        }

        public moduleList(int no,string title,string moduleName,object module)
        {
            moduleNo = no;
            moduleTitle = title;
            moduleClassTitle = moduleName;
            moduleClass = module;
            
        }
    }

}

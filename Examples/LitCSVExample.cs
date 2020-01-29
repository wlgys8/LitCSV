using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MS.LitCSV.Examples{
    public class LitCSVExample : MonoBehaviour
    {
        public TextAsset file1;
        public TextAsset file2;
        

        void Start()
        {
            TestRead(file1);
            TestRead(file2);
        }

        private void TestRead(TextAsset file){
            var csvFile = CSVReader.ReadText(file.text);//read file
            for(var i = 0;i<csvFile.lineCount;i++){
                Debug.LogFormat("Line {0} ===========",i);
                var line = csvFile.GetLine(i);//get line
                for(var j = 0;j<line.cellCount;j++){
                    var str = line.GetCell(j); //get cell
                    Debug.Log(str);
                }
            }
        }
   
    }
}

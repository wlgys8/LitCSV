# LitCSV
 LitCSV for Unity

# Import to Unity

for Unity 2018.3+:

copy below into Packages/manifest.json

    "com.ms.litcsv:https://github.com/wlgys8/LitCSV.git" 

for other untiy version:

Download and drag into project 

# How to Use

    var csvFile = CSVReader.ReadText(file.text);//read file
    var lineCount = csvFile.lineCount; //get line count
    Line line = csvFile.GetLine(0); //get line
    string str = line.GetCell(0);// get cell content





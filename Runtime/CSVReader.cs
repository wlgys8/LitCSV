﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
namespace MS.LitCSV{
    public class CSVReader
    {
       
        private List<Line> _lines;
        private CSVReader(List<Line> lines){
            _lines = lines;
        }

        public int lineCount{
            get{
                return _lines.Count;
            }
        }

        public Line GetLine(int index){
            return _lines[index];
        }

        public string[][] ToArray(){
            string[][] array = new string[lineCount][];
            for(var i = 0; i < array.Length; i ++){
                array[i] = _lines[i].ToArray();
            }
            return array;
        }

        public static CSVReader ReadText(string text){
            var lines = new CSVDecoder(text).Parse();
            return new CSVReader(lines);
        }

        public static CSVReader ReadFile(string filePath){
            var text = File.ReadAllText(filePath);
            return ReadText(text);
        }

    }



    public class Line{

        private List<string> _cells = new List<string>();    

        internal void Add(string cellStr){
            _cells.Add(cellStr);
        }

        public string GetCell(int index){
            return _cells[index];
        }

        public int cellCount{
            get{
                return _cells.Count;
            }
        }

        public string[] ToArray(){
            return _cells.ToArray();
        }

    }

    internal class CSVDecoder{
        const char COMMA = ',';
        const char QUOTE = '"';
        const char NEW_LINE = '\n';
        const char RETURN = '\r';
        private string _text;
        private int _currentCharIndex = 0;
        private bool _inQuote = false;
        private StringBuilder _cellStrBuilder = new StringBuilder();
        private Line _currentLine = null;

        private List<Line> _lines = new List<Line>();

        public CSVDecoder(string content){
            _text = content;
        }

        public List<Line> Parse(){
            while(StepLine()){}
            return _lines;
        }

        private Line currentLine{
            get{
                return _currentLine;
            }
        }

        private bool StepLine(){
            if(_currentCharIndex >= _text.Length){
                return false;
            }
            BeginLine();
            while(StepChar()){}
            EndLine();
            return true;
        }

        private bool PeekNextChar(ref char character){
            if(_currentCharIndex + 1 >= _text.Length){
                return false;
            }
            character = _text[_currentCharIndex + 1];
            return true;
        }

        private char PeekChar(){
            return _text[_currentCharIndex];
        }
        private void EndCell(){
            var cellStr = _cellStrBuilder.ToString();
            currentLine.Add(cellStr);
            _cellStrBuilder.Clear();
        }

        private void BeginLine(){
            _currentLine = new Line();
        }

        private void EndLine(){
            EndCell();
            _lines.Add(_currentLine);
            _currentLine = null;
        }

        private bool StepChar(){
            if(_currentCharIndex >= _text.Length){
                return false;
            }
            var character = PeekChar();
            var lineEnd = false;
            switch(character){
                case NEW_LINE:
                if(!_inQuote){
                    lineEnd = true;
                }else{
                    _cellStrBuilder.Append(character);
                }
                break;
                case RETURN:
                if(!_inQuote){
                    char next = new char();
                    if(PeekNextChar(ref next)){
                        if(next == NEW_LINE){
                            _currentCharIndex ++;//skip for windows
                        }
                    }
                    lineEnd = true;
                    
                }else{
                    _cellStrBuilder.Append(character);
                }
                break;
                case QUOTE:
                if(!_inQuote){
                    _inQuote = true;
                }else{
                    char nextChar = new char();
                    if(PeekNextChar(ref nextChar)){
                        if(nextChar == QUOTE){
                            _cellStrBuilder.Append(nextChar);
                            _currentCharIndex ++;
                        }else{
                            _inQuote = false;
                        }
                    }else{
                        _inQuote = false;
                        // throw new System.Exception("Missing \" at line :" + _lines.Count + "," + _cellStrBuilder.ToString());
                    }
                }
                break;
                case COMMA:
                if(_inQuote){
                    _cellStrBuilder.Append(character);
                }else{
                    EndCell();
                }
                break;
                default:
                _cellStrBuilder.Append(character);
                break;
            }
            _currentCharIndex ++;
            return !lineEnd;
        }

    }
}

/*
	CSVReader by Dock. (24/8/11)
	http://starfruitgames.com
 
	usage: 
	CSVReader.SplitCsvGrid(textString)
 
	returns a 2D string array. 
 
	Drag onto a gameobject for a demo of CSV parsing.
*/
 
using UnityEngine;
using System.Collections;
using System.Linq; 
 
public class CSVReader : MonoBehaviour 
{
	public TextAsset csvFile; 
	public void Start()
	{
		string[,] grid = SplitCsvGrid(csvFile.text);
		Debug.Log("size = " + (1+ grid.GetUpperBound(0)) + "," + (1 + grid.GetUpperBound(1))); 
 
		DebugOutputGrid(grid); 
	}
 
	// outputs the content of a 2D array, useful for checking the importer
	static public void DebugOutputGrid(string[,] grid)
	{
		string textOutput = ""; 
		for (int y = 0; y < grid.GetUpperBound(1); y++) {	
			for (int x = 0; x < grid.GetUpperBound(0); x++) {
 
				textOutput += grid[x,y]; 
				textOutput += "|"; 
			}
			textOutput += "\n"; 
		}
		Debug.Log(textOutput);
	}
 
	// splits a CSV file into a 2D string array
	static public string[,] SplitCsvGrid(string csvText)
	{
		string[] lines = csvText.Split("\n"[0]); 
 
		// finds the max width of row
		int width = 0; 
		for (int i = 0; i < lines.Length; i++)
		{
			string[] row = SplitCsvLine( lines[i] ); 
			width = Mathf.Max(width, row.Length); 
		}
 
		// creates new 2D string grid to output to
		string[,] outputGrid = new string[width + 1, lines.Length + 1]; 
		for (int y = 0; y < lines.Length; y++)
		{
			string[] row = SplitCsvLine( lines[y] ); 
			for (int x = 0; x < row.Length; x++) 
			{
				outputGrid[x,y] = row[x]; 
 
				// This line was to replace "" with " in my output. 
				// Include or edit it as you wish.
				outputGrid[x,y] = outputGrid[x,y].Replace("\"\"", "\"");
			}
		}
 
		return outputGrid; 
	}
 
	// splits a CSV row 
	static public string[] SplitCsvLine(string line)
	{
		return (from System.Text.RegularExpressions.Match m in System.Text.RegularExpressions.Regex.Matches(line,
		@"(((?<x>(?=[,\r\n]+))|""(?<x>([^""]|"""")+)""|(?<x>[^,\r\n]+)),?)", 
		System.Text.RegularExpressions.RegexOptions.ExplicitCapture)
		select m.Groups[1].Value).ToArray();
	}

	//Auxiliary Functions
	static public string[] LineToArray(string path, int lineN){
	//	Debug.Log (path +" "+ lineN );
		TextAsset csv = (TextAsset)Resources.Load(path);
		string[,] grid= CSVReader.SplitCsvGrid(csv.text);
		string[] line = new string[grid.GetLength(0)];
		for(int i=0; i<grid.GetLength(0); i++){
			line[i] = grid[i, lineN];
		}
		return line;
	}


	static public void LineToArray(string path, int lineN, ref string[] s){
		//	Debug.Log (path +" "+ lineN );
		TextAsset csv = (TextAsset)Resources.Load(path);
		string[,] grid= CSVReader.SplitCsvGrid(csv.text);
		//string[] line = new string[grid.GetLength(0)];
		int len = grid.GetLength(0);
		for(int i=0; i<len; i++){
			s[i] = grid[i, lineN];
		}
		if(s.Length>len){
			for(int i=len; i<s.Length; i++){
				s[i]="";
			}
		}
	}


	static public string[,] GridToArray(string path){
		TextAsset csv = (TextAsset)Resources.Load(path);
		return SplitCsvGrid(csv.text);
	}


	//id_column - column to be used as id
	//id_val - value to compare the id to
	//pos_str = the position of column of the string we want to return
	static public string FindArrayOfLineById(string path, int id_column, string id_val, int pos_str){
		TextAsset csv = (TextAsset)Resources.Load(path);

		string[,] arr = CSVReader.SplitCsvGrid(csv.text);
		int lineN=-1;
		for(int i=0; i<arr.GetLength(0); i++){
			lineN = arr[i, id_column].Equals(id_val)? i:-1;
		}

		return arr[lineN, pos_str];

	}


	static public void LoadLanguageCSV(){
		
		GlobalData.Languages.Clear();
		string csv_path = "CSV/Languages";
		TextAsset csv;
		string[,] csvarr = CSVReader.GridToArray(csv_path);
		Debug.Log (csvarr.GetLength(1)-1);
		for(int i=1;i<csvarr.GetLength(1)-1;i++)
		{
			GlobalData.Languages.Add(csvarr[0,i]);
		}
	}
	
	static public string LoadText(int index){
		string textfromcsv="";
		if(GlobalData.Languages.Count>index)
			textfromcsv=GlobalData.Languages[index];
		return textfromcsv;
	}

}
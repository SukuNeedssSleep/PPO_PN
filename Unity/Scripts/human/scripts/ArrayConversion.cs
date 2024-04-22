using UnityEngine;

public class ArrayConversion : MonoBehaviour
{


    public float[,] ConvertStringToArray(string arrayString)
    {   
       
        arrayString = arrayString.Replace(" ","");
        arrayString = arrayString.Replace("'","");
        arrayString = arrayString.Trim('[', ']');
        /*
        string arrayString = "[[1.1, 2.2, 3.3],[4.4, 5.5, 6.6],[7.7, 8.8, 9.9]]";
        */
        // Remove outer brackets
        //print(arrayString);
        // Split by '],[' to get rows
        string[] rowStrings = arrayString.Split(new string[] { "],[" }, System.StringSplitOptions.None);

        // Initialize the result array
        int rows = rowStrings.Length;
        int cols = rowStrings[0].Split(',').Length;
        float[,] resultArray = new float[rows, cols];

        // Parse and populate the result array
        for (int i = 0; i < rows; i++)
        {
            string[] colStrings = rowStrings[i].Split(',');

            // Remove extra spaces in each column string
            for (int j = 0; j < colStrings.Length; j++)
            {
                colStrings[j] = colStrings[j].Trim();
            }

            for (int j = 0; j < cols; j++)
            {
                if (float.TryParse(colStrings[j], out float value))
                {
                    resultArray[i, j] = value;
                }
                else
                {
                    // Handle parsing error, e.g., invalid float format in the string
                    //Debug.LogError($"Error parsing float at [{i},{j}]");
                    return null;
                }
            }
        }
        return resultArray;
    }
}



using AICoursework;


// Reading the file
var listOfCaverns = new List<Cavern>();
var file = Environment.GetCommandLineArgs()[1];
var text = File.ReadAllText(@".\" + file + ".cav");
var values = text.Split(',').ToList();


// Getting the size of the Caverns and printing them out in the console
var numOfCaves = Int32.Parse(values[0]);
values.RemoveAt(0);

// Getting the list of coordinates from the 
var listOfCoordinates = values.GetRange(0,  numOfCaves * 2);
values.RemoveRange(0, numOfCaves * 2);


// Getting the values for the matrix
var matrix = values;


// Getting the list of coordinates
for (var i = 0; i < listOfCoordinates.Count; i += 2)
{
    Cavern cavern = new Cavern(i/2, Int32.Parse(listOfCoordinates[i]), Int32.Parse(listOfCoordinates[i + 1]));
    listOfCaverns.Add(cavern);
}


// Matrix logic that avoids a 2D Array
var toCav = 0;
for (var i = 0; i < matrix.Count; i++)
{

    
    if (Int32.Parse(matrix[i]) != 0)
    {
        var fromCav = (int)Math.Floor((double)i / (double)numOfCaves);
        listOfCaverns[toCav].AddNeighbour(listOfCaverns[fromCav]);
    }

    toCav++;
    if (toCav == numOfCaves)
        toCav = 0;
    
}

// Search algorithm. Reverses the order of the list and prints it to the console
var answers = Search(listOfCaverns);
answers.Reverse();

using (var writer = new StreamWriter(@".\" + file + ".csn"))
{
    if (answers.Count > 0)
    {
        foreach (var a in answers)
        {
            writer.Write(a.Index + " ");
        }

        writer.Write("\n" + answers[^1].GVal);
    }
    else
    {
        writer.Write("0");
    }
}


// Getting Euclidean distance between two caverns
float GetDistanceBetweenCaverns(Cavern fromCavern, Cavern toCavern)
{
    var distance = 0.0f;

    var diffOfX = MathF.Pow(toCavern.Coord[0] - fromCavern.Coord[0], 2);
    var diffOfY = MathF.Pow(toCavern.Coord[1] - fromCavern.Coord[1], 2);

    distance = MathF.Sqrt(diffOfX + diffOfY);
    return distance;

}

// A* Search algorithm
List<Cavern> Search(List<Cavern> caverns)
{

    // This is the list of all the neighbors that we haven't visited yet
    var openSet = new List<Cavern> { caverns[0] };

    // List of all the visited neighbors
    var closedSet = new List<Cavern>();

    // Final path
    var path = new List<Cavern>();


    // While the open set has still values in it
    while (openSet.Count > 0)
    {

        // Getting the lowest F Value in the neighbor list
        var lowestIndex = 0;
        for (var i = 0; i < openSet.Count; i++)
        {
            if (openSet[i].FVal < openSet[lowestIndex].FVal)
            {
                lowestIndex = i;
            }
        }

        // Getting the current cavern with the lowest index
        var current = openSet[lowestIndex];


        // If we arrived at the end of the caverns then build the path
        if (current == caverns[^1])
        {

            var temp = current;
            path.Add(temp);

            // Backtracking using the Parent property
            while (temp.Parent.Index != 1)
            {
                path.Add(temp.Parent);
                temp = temp.Parent;
            }

            path.Add(caverns[0]);
            return path;
        }


        // Remove the current cavern from the open set because we are "visiting" it
        openSet.Remove(current);
        
        // Adding the current cavern to the closed set because we are "visiting" it
        closedSet.Add(current);


        // Getting the list of the current cavern's neighbors
        var neighbors = current.Caverns;

        foreach (var neighbor in neighbors)
        {
            // If we have been to that cavern already, skip everything
            if (closedSet.Contains(neighbor)) continue;

            // Getting the expected G score between the current cavern and the current neighbor
            var tentativeGScore = current.GVal + GetDistanceBetweenCaverns(current, neighbor);

            // Flag that tells if a new path has been found or not
            var newPath = false;

            // If it is already in the open set
            if (openSet.Contains(neighbor))
            {
                // If the current GScore is actually lower than the neighbor's GSCore
                if (tentativeGScore < neighbor.GVal)
                {
                    // Set the neighbor's GSCore to the new GValue
                    neighbor.GVal = tentativeGScore;

                    // And set this to true because we found a better new path
                    newPath = true;
                }

            }
            else
            {
                // If it is not in the open set and it is a new neighbor, then set its GScore and set the new path to true
                neighbor.GVal = tentativeGScore;
                newPath = true;
                openSet.Add(neighbor);
            }

            // This is required as it will ONLY update the heuristic and the F value when a new path or a better path is found
            if (newPath)
            {
                neighbor.HVal = GetDistanceBetweenCaverns(neighbor, caverns[^1]);
                neighbor.FVal = neighbor.GVal + neighbor.HVal;
                neighbor.Parent = current;
            }

        }
    }

    return path;

}


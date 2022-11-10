using System;
using System.Diagnostics;
using AICoursework;

var watch = new Stopwatch();

List<Cavern> listOfCaverns = new List<Cavern>();
Console.WriteLine(System.AppContext.BaseDirectory);
string text = File.ReadAllText(@".\generated5000-1.cav");
List<String> values = text.Split(',').ToList();


int numOfCaves = Int32.Parse(values[0]);
values.RemoveAt(0);
Console.WriteLine("There are " + numOfCaves + " caves");

watch.Start();
List<String> listOfCoordinates = values.GetRange(0,  numOfCaves * 2);
values.RemoveRange(0, numOfCaves * 2);


List<String> matrix = values;


// Getting the list of coordinates
for (int i = 0; i < listOfCoordinates.Count; i += 2)
{
    Cavern cavern = new Cavern(i/2, Int32.Parse(listOfCoordinates[i]), Int32.Parse(listOfCoordinates[i + 1]));
    listOfCaverns.Add(cavern);
}


//// Printing all the list of coordinates
//foreach (var c in listOfCaverns)
//{
//    Console.WriteLine("Cavern coords: " + c.Coord[0] + " " + c.Coord[1]);
//}



for (int i = 0; i < Math.Sqrt(matrix.Count); i++)
{
    for (int j = 0; j < Math.Sqrt(matrix.Count); j++)
    {
        if (Int32.Parse(matrix[i + j * numOfCaves]) == 1)
        {
            listOfCaverns[i].AddNeighbour(listOfCaverns[j]);
        }
    }
}



List<Cavern> answers = Search(listOfCaverns);
answers.Reverse();
double length = 0;
foreach (var answer in answers)
{
    Console.Write(answer.Index + ",");
}

if(answers.Count > 0)
    Console.WriteLine("\n" + answers[^1].GVal);
else
    Console.WriteLine(0);

watch.Stop();
Console.WriteLine("Elapsed milliseconds = " + watch.ElapsedMilliseconds);

float GetDistanceBetweenCaverns(Cavern fromCavern, Cavern toCavern)
{
    float distance = 0.0f;

    float diffOfX = MathF.Pow(toCavern.Coord[0] - fromCavern.Coord[0], 2);
    float diffOfY = MathF.Pow(toCavern.Coord[1] - fromCavern.Coord[1], 2);

    distance = MathF.Sqrt(diffOfX + diffOfY);
    return distance;

}


List<Cavern> Search(List<Cavern> caverns)
{

    var openSet = new List<Cavern> { caverns[0] };
    var closedSet = new List<Cavern>();

    var path = new List<Cavern>();

    while (openSet.Count > 0)
    {
        var lowestIndex = 0;
        for (int i = 0; i < openSet.Count; i++)
        {
            if (openSet[i].FVal < openSet[lowestIndex].FVal)
            {
                lowestIndex = i;
            }
        }

        var current = openSet[lowestIndex];

        if (current == caverns[^1])
        {
            
            var temp = current;
            path.Add(temp);

            while (temp.Parent.Index != 1)
            {
                path.Add(temp.Parent);
                temp = temp.Parent;
            }

            path.Add(caverns[0]);
            return path;
        }
            


        openSet.Remove(current);
        closedSet.Add(current);

        var neighbors = current.Caverns;

        foreach (var neighbor in neighbors)
        {
            if (!closedSet.Contains(neighbor))
            {
                var tentativeGScore = current.GVal + GetDistanceBetweenCaverns(current, neighbor);

                var newPath = false;
                if (openSet.Contains(neighbor))
                {
                    if (tentativeGScore < neighbor.GVal)
                    {
                        neighbor.GVal = tentativeGScore;
                        newPath = true;
                    }
                }
                else
                {
                    neighbor.GVal = tentativeGScore;
                    newPath = true;
                    openSet.Add(neighbor);
                }

                if (newPath)
                {
                    neighbor.HVal = GetDistanceBetweenCaverns(neighbor, caverns[^1]);
                    neighbor.FVal = neighbor.GVal + neighbor.HVal;
                    neighbor.Parent = current;
                }

            }

        }
    }

    return path;

}


double GetHValue(List<Cavern> caverns, Cavern cavern)
{
    return GetDistanceBetweenCaverns(cavern, caverns[^1]);
}

































//Dictionary<Cavern, double> openList = new Dictionary<Cavern, double>();

//List<Cavern> closedSet = new List<Cavern>();

//openList.Add(caverns[0], GetHValue(caverns, caverns[0]));
//while (openList.Count != 0)
//{
//    var current = openList.MinBy(x => x.Value).Key;

//    if (current.Index == caverns[^1].Index)
//        return closedSet;

//    foreach (var neighbor in current.Caverns)
//    {
//        if (closedSet.Contains(neighbor)) 
//            continue;
//        double tentative_gScore = gScore[current] + GetDistanceBetweenCaverns(current, neighbor);


//        double neighbor_gScore = 0;

//        if (!gScore.TryGetValue(neighbor, out neighbor_gScore))
//        {
//            neighbor_gScore = Double.MaxValue;
//        }

//        if (tentative_gScore < neighbor_gScore)
//        {
//            closedSet.Add(neighbor);
//            gScore[neighbor] = tentative_gScore;

//            if (!openList.ContainsKey(neighbor))
//            {
//                openList.Add(neighbor, tentative_gScore + GetHValue(caverns, neighbor));
//            }
//        }

//    }

//    openList.Remove(current);
//}

//return closedSet;
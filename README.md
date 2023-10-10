Another spread-fill algorithm This one scans the space (x,y) to find a point. After that, the points around the found point are scanned. From that, a list of new points is created. The process is controlled by two arrays that are scanned and found. The heart of the algorithm is

       for (int i = 0; i < blop_points.Count; i++)
           if (scaned_points[blop_points[i].X, blop_points[i].Y] == false)
               blop_points.AddRange(ScanNeighburst(blop_points[i]));


   So, have a fun and fill free to use it.

   Daniel
# SpredFill

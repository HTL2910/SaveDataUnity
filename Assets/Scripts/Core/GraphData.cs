// Assets/Scripts/Core/GraphData.cs
using System;
using System.Text;

[Serializable]
public class GraphData
{
    public int N;
    // Ma trận kề: A[i,j] = true nếu có cạnh i<->j (không self-loop)
    public bool[,] A;

    public GraphData(int n)
    {
        N = Math.Max(2, n);
        A = new bool[N, N];
    }

    public bool Get(int i, int j) => A[i, j];
    public void SetUndirected(int i, int j, bool v)
    {
        if (i == j) return;
        A[i, j] = v;
        A[j, i] = v;
    }

    public void Clear()
    {
        for (int i = 0; i < N; i++)
            for (int j = 0; j < N; j++)
                A[i, j] = false;
    }

    public int Degree(int i)
    {
        int d = 0;
        for (int j = 0; j < N; j++) if (A[i, j]) d++;
        return d;
    }

    public int EdgeCount()
    {
        int c = 0;
        for (int i = 0; i < N; i++)
            for (int j = i+1; j < N; j++)
                if (A[i, j]) c++;
        return c;
    }

    // Đếm số tam giác (cycle 3) bằng duyệt tổ hợp – đủ nhanh cho N<=10
    public int TriangleCount()
    {
        int tri = 0;
        for (int i = 0; i < N; i++)
            for (int j = i+1; j < N; j++) if (A[i, j])
                for (int k = j+1; k < N; k++)
                    if (A[i, k] && A[j, k]) tri++;
        return tri;
    }

    //binary string
    public string ToBinaryString()
    {
        var sb = new StringBuilder(N*N);
        for (int i = 0; i < N; i++)
            for (int j = 0; j < N; j++)
                sb.Append(i==j ? '0' : (A[i,j] ? '1' : '0'));
        return $"{N}:{sb}";
    }
    public static GraphData FromBinaryString(string s)
    {
        var parts = s.Split(':');
        int n = int.Parse(parts[0]);
        var bits = parts[1];
        var g = new GraphData(n);
        int t = 0;
        for (int i = 0; i < n; i++)
            for (int j = 0; j < n; j++, t++)
                if (i!=j) g.A[i,j] = bits[t] == '1';
        // đảm bảo đối xứng
        for (int i = 0; i < n; i++)
            for (int j = i+1; j < n; j++)
                g.SetUndirected(i, j, g.A[i,j] || g.A[j,i]);
        return g;
    }
}

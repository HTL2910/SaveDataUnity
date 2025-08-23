using System;
using System.Text;
using System.Collections.Generic;

[Serializable]
public class GraphData
{
    public int m_N;
    // Ma trận kề: A[i,j] = true nếu có cạnh i<->j (không self-loop)
    public bool[,] m_A;

    public GraphData(int n)
    {
        m_N = Math.Max(2, n);
        m_A = new bool[m_N, m_N];
    }

    // Properties for compatibility with MatrixGenerator
    public List<int> nodes
    {
        get
        {
            var result = new List<int>();
            for (int i = 0; i < m_N; i++)
                result.Add(i);
            return result;
        }
    }

    public List<(int, int)> edges
    {
        get
        {
            var result = new List<(int, int)>();
            for (int i = 0; i < m_N; i++)
                for (int j = i + 1; j < m_N; j++)
                    if (m_A[i, j])
                        result.Add((i, j));
            return result;
        }
    }

    public bool Get(int i, int j) 
    {
        if (i < 0 || i >= m_N || j < 0 || j >= m_N) return false;
        return m_A[i, j];
    }

    public void SetUndirected(int i, int j, bool v)
    {
        if (i < 0 || i >= m_N || j < 0 || j >= m_N || i == j) return;
        m_A[i, j] = v;
        m_A[j, i] = v;
    }

    public void Clear()
    {
        for (int i = 0; i < m_N; i++)
            for (int j = 0; j < m_N; j++)
                m_A[i, j] = false;
    }

    public int Degree(int i)
    {
        if (i < 0 || i >= m_N) return 0;
        int d = 0;
        for (int j = 0; j < m_N; j++)
            if (m_A[i, j]) d++;
        return d;
    }

    public int EdgeCount()
    {
        int c = 0;
        for (int i = 0; i < m_N; i++)
            for (int j = i + 1; j < m_N; j++)
                if (m_A[i, j]) c++;
        return c;
    }

    // Đếm số tam giác (cycle 3) bằng duyệt tổ hợp – đủ nhanh cho N<=10
    public int TriangleCount()
    {
        int tri = 0;
        for (int i = 0; i < m_N; i++)
            for (int j = i + 1; j < m_N; j++) 
                if (m_A[i, j])
                    for (int k = j + 1; k < m_N; k++)
                        if (m_A[i, k] && m_A[j, k]) tri++;
        return tri;
    }

    // === Các API tương thích với MatrixGenerator ===
    public void AddEdge(int a, int b)
    {
        SetUndirected(a, b, true);
    }

    public bool IsConnected(int a, int b)
    {
        return Get(a, b);
    }

    // Method for MatrixGenerator compatibility
    public void AddNode(int nodeId)
    {
        // Nodes are automatically created when GraphData is initialized
        // This method exists for compatibility but doesn't need to do anything
        // since the graph size is fixed at construction
    }

    // Check if the entire graph is connected (all nodes reachable)
    public bool IsGraphConnected()
    {
        if (m_N <= 1) return true;
        
        // Use DFS to check connectivity
        bool[] visited = new bool[m_N];
        DFS(0, visited);
        
        // Check if all nodes were visited
        for (int i = 0; i < m_N; i++)
            if (!visited[i]) return false;
        
        return true;
    }

    private void DFS(int node, bool[] visited)
    {
        visited[node] = true;
        for (int i = 0; i < m_N; i++)
        {
            if (m_A[node, i] && !visited[i])
            {
                DFS(i, visited);
            }
        }
    }

    //binary string
    public string ToBinaryString()
    {
        var sb = new StringBuilder(m_N * m_N);
        for (int i = 0; i < m_N; i++)
            for (int j = 0; j < m_N; j++)
                sb.Append(i == j ? '0' : (m_A[i, j] ? '1' : '0'));
        return $"{m_N}:{sb}";
    }

    public static GraphData FromBinaryString(string s)
    {
        if (string.IsNullOrEmpty(s)) return new GraphData(2);
        
        var parts = s.Split(':');
        if (parts.Length != 2) return new GraphData(2);
        
        if (!int.TryParse(parts[0], out int n) || n < 2) return new GraphData(2);
        
        var bits = parts[1];
        if (bits.Length != n * n) return new GraphData(n);
        
        var g = new GraphData(n);
        int t = 0;
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++, t++)
            {
                if (i != j && t < bits.Length)
                {
                    g.m_A[i, j] = bits[t] == '1';
                }
            }
        }
        
        // Đảm bảo ma trận đối xứng
        for (int i = 0; i < n; i++)
        {
            for (int j = i + 1; j < n; j++)
            {
                bool hasEdge = g.m_A[i, j] || g.m_A[j, i];
                g.m_A[i, j] = hasEdge;
                g.m_A[j, i] = hasEdge;
            }
        }
        
        return g;
    }
}

// using System.Collections;
// using System.Collections.Generic;
// using System.Diagnostics;
// using UnityEngine;


// public class TestLinAlg
// {
//     // public List<float> vecMatMult(List<float> a, List<List<float>> B){
//     //     // a = 1 x N 
//     //     // B = N x K

//     //     List<float> c = new List<float>(new float[B[0].Count]);
//     //     for(int i = 0; i < B[0].Count; i++){
//     //         for(int j = 0; j < B.Count; j++){
//     //             c[i] += a[j] * B[j][i];
//     //         }
//     //     }
//     //     return c; 
//     // }

//     // public List<List<float>> transpose(List<List<float>> A){
//     //     List<List<float>> AT = new List<List<float>>(A[0].Count); 

//     //     for(int i = 0; i < A[0].Count; i++){
//     //         AT.Add(new List<float>(new float[A.Count])); 
//     //         for(int j = 0; j < A.Count; j++){
//     //             AT[i][j] = A[j][i];
//     //         }
//     //     }
//     //     return AT; 
//     // }

//     // public List<float> add(List<float> a, List<float> b){
//     //     List<float> c = new List<float>(new float[a.Count]);
//     //     for(int i = 0; i < c.Count; i++){
//     //         c[i] = a[i] + b[i];
//     //     }
//     //     return c; 
//     // }

//     // public List<List<float>> add(List<List<float>> A, List<List<float>> B){
//     //     List<List<float>> C = new List<List<float>>(A.Count);
//     //     for(int i = 0; i < A.Count; i++){
//     //         C.Add(new List<float>(new float[A[0].Count]));
//     //         for(int j = 0; j < A[0].Count; j++){
//     //             C[i][j] = A[i][j] + B[i][j];
//     //         }
//     //     }
//     //     return C; 
//     // }

//     // public List<float> subtract(List<float> a, List<float> b){
//     //     List<float> c = new List<float>(new float[a.Count]);
//     //     for(int i = 0; i < c.Count; i++){
//     //         c[i] = a[i] - b[i];
//     //     }
//     //     return c; 
//     // }

//     // public List<List<float>> subtract(List<List<float>> A, List<List<float>> B){
//     //     List<List<float>> C = new List<List<float>>(A.Count);
//     //     for(int i = 0; i < A.Count; i++){
//     //         C.Add(new List<float>(new float[A[0].Count]));
//     //         for(int j = 0; j < A[0].Count; j++){
//     //             C[i][j] = A[i][j] - B[i][j];
//     //         }
//     //     }
//     //     return C; 
//     // }


//     // public List<float> hadamard(List<float> a, List<float> b){
//     //     List<float> c = new List<float>(new float[a.Count]);
//     //     for(int i = 0; i < c.Count; i++){
//     //         c[i] = a[i] * b[i];
//     //     }
//     //     return c; 
//     // }

//     LinAlg linalg; 
//     public float testDot(){

//         List<float> a = new List<float>() { 1f, 2f, 3f };
//         List<float> b = new List<float>() { 2f, 4f, 6f };

//         float expectedDot = 28; 
//         Debug.Assert.AreEqual(linalg.dot(a, b)); 

//         Debug.Assert.AreEqual(a, a);
//         Debug.Assert.AreEqual(b, b);

//     }

//     // public List<List<float>> outerProduct(List<float> a, List<float> b){
//     //     List<List<float>> C = new List<List<float>>(a.Count);
//     //     for(int i = 0; i < a.Count; i++){
//     //         C.Add(new List<float>(new float[b.Count]));
//     //         C[i] = scalarMultiply(b, a[i]);
//     //     }
//     //     return C;
//     // }

//     // public List<float> scalarMultiply(List<float> a, float s){
//     //     List<float> b = new List<float>(new float[a.Count]);
//     //     for(int i = 0; i < a.Count; i++){
//     //         b[i] = (a[i] * s); 
//     //     }
//     //     return b; 
//     // }

//     // public List<List<float>> scalarMultiply(List<List<float>> A, float s){
//     //     List<List<float>> B = new List<List<float>>(A.Count);
//     //     for(int i = 0; i < A.Count; i++){
//     //         B.Add(new List<float>(new float[A[0].Count]));
//     //         for(int j = 0; j < A[i].Count; j++){
//     //             B[i][j] = A[i][j] * s;
//     //         }
//     //     }
//     //     return B; 
//     // }
// }

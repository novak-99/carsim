using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random=System.Random;
using System.IO;
//using RLCar; 

public class NeuralNetwork
{
    LinAlg linalg = new LinAlg();
    Random random = new Random();
    private static int k = 2;  
    private static int hidden = 3;
    private static float gamma = 0.99f; 

    List<float> mu = new List<float>();
    List<float> sigma = new List<float>();

    List<List<float>> a = new List<List<float>>(); 
    List<List<float>> z = new List<List<float>>(); 

    public List<float> weightsMu = new List<float>(new float[hidden]);
    float biasMu; 

    List<float> weightsSigma = new List<float>(new float[hidden]);
    float biasSigma; 

    List<List<float>> weightsHidden = new List<List<float>>(k);

    List<float> biasHidden = new List<float>(new float[hidden]); // Initialize to 0... 


    // redo with correct uniform [0,1] initialization ... for mu, sigma, hidden.
    public NeuralNetwork(){
        // ((float)(random.NextDouble() * 2 - 1));
        // ((float)(random.NextDouble() * 2 * limit - limit));
        // biasMu =((float)(random.NextDouble() * 2));
        // biasSigma = ((float)(random.NextDouble() * 2));
        for(int i = 0; i < k; i++){
            // float limit = Mathf.Sqrt(6 / k); 
            weightsHidden.Add(new List<float>(new float[hidden])); // k x hidden

            for(int j = 0; j < hidden; j++){
                weightsHidden[i][j] = ((float)(random.NextDouble() ));
            }
        }

        for(int i = 0; i < hidden; i++){
            // float limit = Mathf.Sqrt(6 / hidden); 
            weightsMu[i] = ((float)(random.NextDouble() ));
            weightsSigma[i] = ((float)(random.NextDouble() ));
            // biasHidden[i] = ((float)(random.NextDouble() * 2));
        }



        // 3 x 2

        List<List<float>> mat = new List<List<float>>()
        {
            new List<float>() { 1f, 2f },
            new List<float>() { 3f, 4f },
            new List<float>() { 5f, 6f }
        };


        // 3 x 1
        List<float> vec = new List<float>() { 1f, 2f, 3f };

        // List<List<float>> final = linalg.add(mat, mat); 
        List<float> final = linalg.hadamard(vec, vec); 


        // string str = ""; 
        // for(int i = 0; i < final.Count; i++){
        //     str += vec[i] + " "; 
        // }
        // Debug.Log(str);

        // Debug.Log(linalg.dot(vec, vec));


        // for(int i = 0; i < final.Count; i++){
        //     string str = ""; 
        //     for(int j = 0; j < final[0].Count; j++){
        //         str += final[i][j] + " "; 
        //     }
        //     Debug.Log(str);
        // }

    }

    List<float> ReLU(List<float> z, bool deriv = false){
        if(!deriv){
            List<float> a = new List<float>(new float[z.Count]);
            for(int i = 0; i < z.Count; i++){
                a[i] = Mathf.Max(0, z[i]);
            }
            return a; 
        }
        else{
            List<float> reluDeriv = new List<float>(new float[z.Count]);
            for(int i = 0; i < z.Count; i++){
                if(z[i] <= 0){
                    reluDeriv[i] = 0; 
                }
                else{
                    reluDeriv[i] = 1; 
                }
            }
            return reluDeriv; 
        }
    } 

    List<float> Tanh(List<float> z, bool deriv = false){
        List<float> a = new List<float>(new float[z.Count]);
        for(int i = 0; i < z.Count; i++){
            a[i] = ( Mathf.Exp(z[i]) - Mathf.Exp(-z[i]) ) / ( Mathf.Exp(z[i]) + Mathf.Exp(-z[i]) );
        }
        if(!deriv) return a; 
        else{
            for(int i = 0; i < z.Count; i++){
                a[i] = 1 - a[i] * a[i]; 
            }   
            return a; // but to be clear a is a'
        }
    } 

    List<float> Sigmoid(List<float> z, bool deriv = false){
        List<float> a = new List<float>(new float[z.Count]);
        for(int i = 0; i < z.Count; i++){
            a[i] = 1 / (1 + Mathf.Exp(-z[i]));
        }
        if(!deriv) return a; 
        else{
            for(int i = 0; i < z.Count; i++){
                a[i] = a[i] * (1 - a[i]);
            }   
            return a; // a is a'... 
        }
    } 

    // fix this .. 
    List<float> weightedReward(List<float> rewards){
        List<float> R = new List<float>(new float[rewards.Count]);
        for(int i = 0; i < rewards.Count; i++){
            R[i] = rewards[i];
            for(int j = i+1; j < rewards.Count; j++){
                R[i] += Mathf.Pow(gamma, j-i) * rewards[j]; 
            }
        }
        return R; 
    }

    // logloss. 
    // so we basically only do this after mu and sigma have been instantiated w/eval.
    // float logLoss(List<float> state, float action, float R){
    //     return -Mathf.Log(gaussianPDF(action, mu, sigma)) * R; 
    // }

    // make testing equivalents of both. 
    // this is the primary objective 
    public void evaluateP(List<float> state){
        z.Add(linalg.add(linalg.vecMatMult(state, weightsHidden), biasHidden)); 
        a.Add(Sigmoid(z[z.Count - 1]));

        mu.Add(linalg.dot(a[a.Count - 1], weightsMu) + biasMu); 
        sigma.Add(linalg.dot(a[a.Count - 1], weightsSigma) + biasSigma); 
    }

    public float sampleAngle(){
        // Box-Muller Transform 
        // Taken from: https://stackoverflow.com/questions/218060/random-gaussian-variables

        float U1 = (float)(1.0 - random.NextDouble()); //uniform(0,1] random doubles
        float U2 = (float)(1.0 - random.NextDouble());
        float Z1 = Mathf.Sqrt(-2.0f * Mathf.Log(U1)) * Mathf.Sin(2.0f * Mathf.PI * U2); //random normal(0,1)
        return mu[mu.Count - 1] + sigma[sigma.Count - 1] * Z1; //random normal(mean,stdDev^2) 
    }

    float gaussianPDF(float action, float mu, float sigma){
        float exp = Mathf.Exp(-0.5f * Mathf.Pow( (action - mu)/sigma, 2) );
        return exp/(Mathf.Sqrt(2.0f * Mathf.PI) * sigma);
    }

    void printVector(List<float> a){
        string str = ""; 
        for(int i = 0; i < a.Count; i++){
            str += a[i] + " "; 
        }
        Debug.Log(str);
    }

    void printMatrix(List<List<float>> A){
        for(int i = 0; i < A.Count; i++){
            printVector(A[i]);
        }
    }

    // normal, vanilla GD. 

    public void train(List<List<float>> states, List<float> actions, List<float> rewards){
        int T = states.Count; 
        float lr = 0.001f * Mathf.Pow(0.999f, T); 
        Debug.Log("LR: " + lr); 
        List<float> R = weightedReward(rewards); 
        for(int t = 0; t < T; t++){
            //evaluateP(states[t], actions[t]); // this initializes all variables. 
            // part of all gradients. 
            float pdf = gaussianPDF(actions[t], mu[t], sigma[t]); 
            float constantMultiplier = R[t] / pdf / T; 

            float expTerm = Mathf.Exp(-0.5f * Mathf.Pow((actions[t] - mu[t]), 2) / Mathf.Pow(sigma[t], 2)); 

            // Update mu variables
            float pdfMuGrad = expTerm * (actions[t] - mu[t]) / ( Mathf.Pow(sigma[t], 3) * Mathf.Sqrt(2 * Mathf.PI)); 

            List<float> weightsMuGrad = linalg.scalarMultiply(a[t], pdfMuGrad * constantMultiplier);
            weightsMu = linalg.add(weightsMu, linalg.scalarMultiply(weightsMuGrad, lr));
            biasMu += lr * pdfMuGrad * constantMultiplier; 


            // if(t == T - 1){
            //     Debug.Log("Weights Mu Grad:");
            //     printVector(linalg.scalarMultiply(weightsMuGrad, lr)); 

            //     Debug.Log("Bias Mu Grad:");
            //     Debug.Log(lr * pdfMuGrad * constantMultiplier); 
            // }

            // Update sigma variables 
            float pdfSigmaGrad = expTerm * ( Mathf.Pow(actions[t] - mu[t], 2) / (Mathf.Pow(sigma[t], 4) * Mathf.Sqrt(2 * Mathf.PI)) - 1 / (Mathf.Pow(sigma[t], 2) * Mathf.Sqrt(2 * Mathf.PI)) );

            List<float> weightsSigmaGrad = linalg.scalarMultiply(a[t], pdfSigmaGrad * constantMultiplier);
            weightsSigma = linalg.add(weightsSigma, linalg.scalarMultiply(weightsSigmaGrad, lr));
            biasSigma += lr * pdfSigmaGrad * constantMultiplier; 

            // if(t == T - 1){
            //     Debug.Log("Weights Sigma Grad:");
            //     printVector(linalg.scalarMultiply(weightsSigmaGrad, lr)); 

            //     Debug.Log("Bias Sigma Grad:");
            //     Debug.Log( lr * pdfSigmaGrad * constantMultiplier); 
            // }

            // Update hidden layer weights
            List<float> gradientSum = linalg.add(linalg.scalarMultiply(weightsSigma, pdfSigmaGrad), linalg.scalarMultiply(weightsMu, pdfMuGrad));
            List<float> reluDeriv = Sigmoid(z[t], true); // deriv == true means ReLU'(z)! 

            List<List<float>> weightsHiddenGrad  = linalg.outerProduct(states[t], linalg.hadamard(gradientSum, reluDeriv));
            weightsHidden = linalg.add(weightsHidden, linalg.scalarMultiply(weightsHiddenGrad, lr * constantMultiplier));
            biasHidden = linalg.add(biasHidden, linalg.scalarMultiply(linalg.hadamard(gradientSum, reluDeriv), lr * constantMultiplier));

            // if(t == T - 1){
            //     Debug.Log("Weights Hidden Grad:");
            //     printMatrix(linalg.scalarMultiply(weightsHiddenGrad, lr * constantMultiplier)); 

            //     Debug.Log("Bias Hidden Grad:");
            //     printVector(linalg.scalarMultiply(linalg.hadamard(gradientSum, reluDeriv), lr * constantMultiplier)); 
            // }
        }

        mu = new List<float>();
        sigma = new List<float>();

        a = new List<List<float>>(); 
        z = new List<List<float>>(); 
    }

    public void saveWeights(){
        using (StreamWriter sw = new StreamWriter(Application.dataPath + "/Weights.txt", false))
        {
            sw.WriteLine("Layer 2 Parameters");
            sw.WriteLine("Mu Weights");
            for(int i = 0; i < weightsMu.Count; i++){
                sw.Write(weightsMu[i]); 
                sw.Write(" ");
            }
            sw.WriteLine();

            sw.WriteLine("Mu Bias");
            sw.Write(biasMu); 
            sw.WriteLine();

            sw.WriteLine("Sigma Weights");
            for(int i = 0; i < weightsSigma.Count; i++){
                sw.Write(weightsSigma[i]); 
                sw.Write(" ");
            }
            sw.WriteLine();

            sw.WriteLine("Sigma Bias");
            sw.Write(biasSigma); 
            sw.WriteLine();

            sw.WriteLine("Layer 1 Parameters");
            sw.WriteLine("Hidden Weights");
            for(int i = 0; i < weightsHidden.Count; i++){
                for(int j = 0; j < weightsHidden[i].Count; j++){
                    sw.Write(weightsHidden[i][j]); 
                    sw.Write(" ");
                }
                sw.WriteLine();
            }
            sw.WriteLine("Hidden Bias");
            for(int i = 0; i < biasHidden.Count; i++){
                sw.Write(biasHidden[i]); 
                sw.Write(" ");
            }
        }
    }

    public void loadWeights(string fileName){
        using (StreamReader sr = new StreamReader(Application.dataPath + "/" + fileName, false)){
            string line = sr.ReadLine(); // empty
            line = sr.ReadLine(); // empty

            line = sr.ReadLine(); // Mu Weights
            string[] words = line.Split(' ');

            for(int i = 0; i < hidden; i++){
                weightsMu[i] = float.Parse(words[i]);
            }

            line = sr.ReadLine(); // empty 

            line = sr.ReadLine(); // mu bias 
            words = line.Split(' ');
            biasMu = float.Parse(words[0]); 

            line = sr.ReadLine(); // empty

            line = sr.ReadLine(); // sigma Weights
            words = line.Split(' ');

            for(int i = 0; i < hidden; i++){
                weightsSigma[i] = float.Parse(words[i]);
            }

            line = sr.ReadLine(); // empty 

            line = sr.ReadLine(); // sigma bias 
            words = line.Split(' ');
            biasSigma = float.Parse(words[0]); 

            line = sr.ReadLine(); // empty 
            line = sr.ReadLine(); // empty 

            for(int i = 0; i < k; i++){
                line = sr.ReadLine(); // this is a row of hidden weights
                words = line.Split(' ');

                for(int j = 0; j < hidden; j++){
                    weightsHidden[i][j] = float.Parse(words[j]); 
                }

            }

            line = sr.ReadLine(); // empty 
            line = sr.ReadLine(); //  hidden bias
            words = line.Split(' ');

            for(int i = 0; i < hidden; i++){
                biasHidden[i] = float.Parse(words[i]);
            }
        }   
    }
}
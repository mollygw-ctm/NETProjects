using Microsoft.ML;
using TaxiFarePrediction;

// path to file with the data set used to train the model
string _trainDataPath = Path.Combine(Environment.CurrentDirectory, "Data", "taxi-fare-train.csv");

// path to the file with the data set used to evaluate the model
string _testDataPath = Path.Combine(Environment.CurrentDirectory, "Data", "taxi-fare-test.csv");

// path to the file where the trained model is stored
string _modelPath = Path.Combine(Environment.CurrentDirectory, "Data", "Model.zip");

MLContext mlContext = new MLContext(seed: 0);

var model = Train(mlContext, _trainDataPath);

ITransformer Train(MLContext mlContext, string dataPath)
{
    IDataView dataView = mlContext.Data.LoadFromTextFile<TaxiTrip>(dataPath, hasHeader: true, separatorChar: ',');
    var pipeline = mlContext.Transforms.CopyColumns(outputColumnName: "Label", inputColumnName: "FareAmount")
        // amends data so it is numeric
        .Append(mlContext.Transforms.Categorical.OneHotEncoding(outputColumnName: "VendorIdEncoded", inputColumnName: "VendorId"))
        .Append(mlContext.Transforms.Categorical.OneHotEncoding(outputColumnName: "RateCodeEncoded", inputColumnName: "RateCode"))
        .Append(mlContext.Transforms.Categorical.OneHotEncoding(outputColumnName: "PaymentTypeEncoded", inputColumnName: "PaymentType"))
        .Append(mlContext.Transforms.Concatenate("Features", "VendorIdEncoded", "RateCodeEncoded", "PassengerCount", "TripDistance", "PaymentTypeEncoded"))
        // implements FastTree which uses gradient boosting where multiple decision trees are combined to make accurate predictions
        .Append(mlContext.Regression.Trainers.FastTree());

    var model = pipeline.Fit(dataView);

    return model;
}

Evaluate(mlContext, model);

void Evaluate(MLContext mlContext, ITransformer model)
{
    // Loads data
    IDataView dataView = mlContext.Data.LoadFromTextFile<TaxiTrip>(_testDataPath, hasHeader: true, separatorChar: ',');
    // transform the test data
    var predictions = model.Transform(dataView);

    var metrics = mlContext.Regression.Evaluate(predictions, "Label", "Score");

    Console.WriteLine();
    Console.WriteLine($"*************************************************");
    Console.WriteLine($"*       Model quality metrics evaluation         ");
    Console.WriteLine($"*------------------------------------------------");
    Console.WriteLine($"*       RSquared Score:      {metrics.RSquared:0.##}");
    Console.WriteLine($"*       Root Mean Squared Error:      {metrics.RootMeanSquaredError:#.##}");
}

TestSinglePrediction(mlContext, model);

void TestSinglePrediction(MLContext mlContext, ITransformer model)
{
    // CreatePredictionEngine is a convenience API which allows perforamnce of a prediction on a single instance of data
    var predictionFunction = mlContext.Model.CreatePredictionEngine<TaxiTrip, TaxiTripFarePrediction>(model);

    var taxiTripSample = new TaxiTrip()
    {
        VendorId = "VTS",
        RateCode = "1",
        PassengerCount = 1,
        TripTime = 1140,
        TripDistance = 3.75f,
        PaymentType = "CRD",
        FareAmount = 0 // To predict. Actual/Observed = 15.5
    };

    var prediction = predictionFunction.Predict(taxiTripSample);

    Console.WriteLine(taxiTripSample);

    Console.WriteLine($"**********************************************************************");
    Console.WriteLine($"Predicted fare: {prediction.FareAmount:0.####}, actual fare: 15.5");
    Console.WriteLine($"**********************************************************************");
}
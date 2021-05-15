using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

using MongoDB.Bson;
using MongoDB.Driver;


namespace LibTradutorNetFramework
{
    

public class MongoDb
{
    private IMongoDatabase db;

    public MongoDb(string database)
    {
            //"mongodb+srv://<username>:<password>@cluster0.orjcd.azure.mongodb.net/myFirstDatabase?retryWrites=true&w=majority"
            //mongodb://localhost:27017/?readPreference=primary&appname=MongoDB%20Compass&ssl=false
            //MongoClient client = new MongoClient("mongodb+srv://Jesse:VaThklsWs7i9j1SH@cluster0-fasvt.mongodb.net");
            MongoClient client = new MongoClient("mongodb://localhost:27017/?readPreference=primary&appname=MongoDB%20Compass&ssl=false");

            // a anotação abaixo serve para mapear classes que agem de maneira diferente
            // On Error Resume Next
            // BsonClassMap.RegisterClassMap(Of Pacote)()
            // BsonClassMap.RegisterClassMap(Of PacoteNossoModoPlus)()
            // On Error GoTo 0

            db = client.GetDatabase(database);
    }

    public void InserRecord<T>(string table, T record)
    {
        var collection = db.GetCollection<T>(table);
        collection.InsertOne(record);
    }

    public object LoadRecords<T>(string table)
    {
        var collection = db.GetCollection<T>(table);
        return collection.Find(new BsonDocument()).ToList();
    }

    public object LoadRecordById<T>(string table, Guid id)
    {
        var collection = db.GetCollection<T>(table);
        var filter = Builders<T>.Filter.Eq<Guid>("Id", id);
        return collection.Find(filter).First();
    }

    public void UpsertRecor<T>(string table, ObjectId id, T record)
    {
        var collection = db.GetCollection<T>(table);
        var result = collection.ReplaceOne(new BsonDocument("_id", id), record, new UpdateOptions() { IsUpsert = true });
    }

    public void DeleRecord<T>(string table, Guid id)
    {
        var collection = db.GetCollection<T>(table);
        var filter = Builders<T>.Filter.Eq<Guid>("Id", id);
        collection.DeleteOne(filter);
    }

    public void UpsertRecor<T>(string table, string CNPJ, T record)
    {
        var collection = db.GetCollection<T>(table);
        var result = collection.ReplaceOne(new BsonDocument("CNPJ", CNPJ), record, new UpdateOptions() { IsUpsert = true });
    }

    public void DeleRecord<T>(string table, string CNPJ)
    {
        var collection = db.GetCollection<T>(table);
        var filter = Builders<T>.Filter.Eq<string>("CNPJ", CNPJ);
        collection.DeleteOne(filter);
    }

    public List<string> ObterTraducoesProntas()
    {
        var Traducoes = db.ListCollectionsAsync().Result.ToListAsync<BsonDocument>().Result.ToList<BsonDocument>();

            List<string> output = new List<string>();

            
            foreach (BsonDocument collection in db.ListCollectionsAsync().Result.ToListAsync<BsonDocument>().Result)
            {
                string name = collection["name"].AsString;
                output.Add(name);
            }


            return output;
    }



}
}
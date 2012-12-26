using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Mono.Data.Sqlite;

public class DBAccess : System.Object
{
	protected SqliteConnection dbConnection;
	protected SqliteCommand dbCommand;
	protected SqliteDataReader dbReader;
	
	public DBAccess()
	{
		OpenDB();
	}
	
	public void OpenDB()
	{
		// Set the connection path and open the database
		if( Application.platform == RuntimePlatform.IPhonePlayer )
		{
			dbConnection = new SqliteConnection( "URI=file:" + Application.dataPath + "/../Resources/diceBaseball.sqlite" );
		}
		else
		{
			dbConnection = new SqliteConnection( "URI=file:" + Application.dataPath + "/Resources/diceBaseball.sqlite" );
		}
		
		dbConnection.Open();
	}
	
	public void CloseDB()
	{
		dbReader.Close();
		// TODO: Need to Dispose/Close the command. Not sure how to do it with this version.
		// Reference says dispose but that command is not available...
		dbConnection.Close();
	}
	
	#region Tables
	public void CreateTable( string name, string[] column, string[] columnType )
	{
		// TODO: Check if the table exists here or somewhere else. Still have to write the code though.
		
		string query = "CREATE TABLE " + name + "(" + column[0] + " " + columnType[0];
		
		for( int i = 1; i < column.Length; i++ )
		{
			query += ", " + column[i] + columnType[i];
		}
		
		query += ")";
		
		executeCommand( query );
	}
	
	public void EmptyTable( string tableName )
	{
		string query = "DELETE FROM " + tableName;
		executeCommand( query );
	}
	
	#endregion
	
	#region Insertion
	public void InsertIntoSingle( string tableName, string columnName, string inValue )
	{
		string query = "INSERT INTO " + tableName + " (" + columnName + ") " + "VALUES (\'" + inValue + "\')";
		
		executeCommand( query );
	}
	
	public void InsertIntoSpecific( string tableName, string[] columnName, string[] values )
	{
		string query = "INSERT INTO " + tableName + " (" + columnName[0];
		
		for( int i = 1; i < columnName.Length; i++ )
		{
			query += ", " + columnName[i];
		}
		
		query += ") VALUES (\'" + values[0] + "\'";
		
		for( int i = 1; i < values.Length; i++ )
		{
			query += ", \'" + values[i] + "\'";
		}
		
		query += ")";
		
		executeCommand( query );
	}
	
	public void InsertInto( string tableName, string[] values )
	{
		string query = "INSERT INTO " + tableName + " VALUES (\'" + values[0] + "\'";
		for( int i = 1; i < values.Length; i++ )
		{
			query += ", \'" + values[i] + "\'"; 
		}
		query += ")";
		
		executeCommand( query );
	}
	
	public void ReplaceIntoSingle( string tableName, string columnName, string inValue )
	{
		string query = "REPLACE INTO " + tableName + "(" + columnName + ")" + " VALUES (\'" + inValue + "\')";
		executeCommand(query);
	}
	
	public void UpdateRecordWhere( string tableName, string columnName, string inValue, string refColumnName, string refRowName)
	{
		string query = "UPDATE " + tableName + " SET " + columnName + "=\'" + inValue + "\' WHERE " + refColumnName + "=" + "\'" + refRowName + "\'";
		Debug.Log( query );
		executeCommand( query );
	}
	
	#endregion
	
	#region Select
	public string SelectFirst( string tableName, string columnName )
	{
		string query = "SELECT (" + columnName + ") FROM " + tableName + " LIMIT 1";
		executeCommand( query );
		
		dbReader.Read();
		return dbReader.GetString(0);
	}
	
	public string[] SelectSingle( string tableName, string itemToSelect, string withColumn, string withComparison, string withValue )
	{
		string query = "SELECT " + itemToSelect + " FROM " + tableName + " WHERE " + withColumn + withComparison + withValue;
		
		executeCommand( query );
		
		List<string> matchesList = new List<string>();
		
		// Fill the list with all the matches
		while (dbReader.Read())
		{
			matchesList.Add( dbReader.GetString(0) );
		}
		
		// Convert the matchesList to an array in case the code using this class did not import System.Collections.Generic
		string[] matches = matchesList.ToArray();
		
		return matches;
	}
	
	#endregion
	
	public SqliteDataReader basicQuery( string command, bool returnReader )
	{
		executeCommand( command );
		if( returnReader )
		{
			return dbReader;
		}
		else
		{
			return null;
		} 
	}
	
	private void executeCommand( string command )
	{
		dbCommand = dbConnection.CreateCommand();
		dbCommand.CommandText = command;
		
		// Execute command which returns a reader
		dbReader = dbCommand.ExecuteReader();
	}
}

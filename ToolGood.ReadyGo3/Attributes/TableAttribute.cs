﻿using System;

namespace ToolGood.ReadyGo3.Attributes
{
    /// <summary>
    /// 表特征
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class TableAttribute : Attribute
    {
        /// <summary>
        /// 数据表名
        /// </summary>
        public string TableName;

        /// <summary>
        /// Schema名
        /// </summary>
        public string SchemaName;

        /// <summary>
        /// 数据表
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="fixTag"></param>
        public TableAttribute(string tableName)
        {
            TableName = tableName.Trim();
        }
        /// <summary>
        /// 数据表
        /// </summary>
        /// <param name="schemaName"></param>
        /// <param name="tableName"></param>
        /// <param name="fixTag"></param>
        public TableAttribute(string tableName, string schemaName)
        {
            SchemaName = schemaName.Trim();
            TableName = tableName.Trim();
        }
    }
}
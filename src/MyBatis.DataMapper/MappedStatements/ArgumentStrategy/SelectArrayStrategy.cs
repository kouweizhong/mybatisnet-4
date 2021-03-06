#region Apache Notice
/*****************************************************************************
 * $Revision: 374175 $
 * $LastChangedDate: 2008-06-28 09:26:16 -0600 (Sat, 28 Jun 2008) $
 * $LastChangedBy: gbayon $
 * 
 * iBATIS.NET Data Mapper
 * Copyright (C) 2008/2005 - The Apache Software Foundation
 *  
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *      http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * 
 ********************************************************************************/
#endregion

using System;
using System.Collections;
using System.Data;
using MyBatis.DataMapper.Data;
using MyBatis.DataMapper.Model.ResultMapping;
using MyBatis.DataMapper.Scope;

namespace MyBatis.DataMapper.MappedStatements.ArgumentStrategy
{
	/// <summary>
	/// <see cref="IArgumentStrategy"/> implementation when a 'select' attribute exists
	/// on a <see cref="Array"/> <see cref="ArgumentProperty"/>
	/// </summary>
    public sealed class SelectArrayStrategy : IArgumentStrategy
	{
		#region IArgumentStrategy Members

		/// <summary>
		/// Gets the value of an argument constructor.
		/// </summary>
		/// <param name="request">The current <see cref="RequestScope"/>.</param>
		/// <param name="mapping">The <see cref="ResultProperty"/> with the argument infos.</param>
		/// <param name="reader">The current <see cref="IDataReader"/>.</param>
		/// <param name="keys">The keys</param>
		/// <returns>The paremeter value.</returns>
		public object GetValue(RequestScope request, ResultProperty mapping, 
		                       ref IDataReader reader, object keys)
		{
			// Get the select statement
            IMappedStatement selectStatement = request.MappedStatement.ModelStore.GetMappedStatement(mapping.Select);

            reader = DataReaderTransformer.Transform(reader, request.Session.SessionFactory.DataSource.DbProvider);
			IList values = selectStatement.ExecuteQueryForList(request.Session, keys); 

			Type elementType = mapping.MemberType.GetElementType();
			Array array = Array.CreateInstance(elementType, values.Count);
			int count = values.Count;
			for(int i=0;i<count;i++)
			{
				array.SetValue(values[i],i);
			}
			return array;
		}

		#endregion
	}
}

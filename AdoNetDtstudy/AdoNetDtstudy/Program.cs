using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoNetDtstudy
{
    class Program
    {
        private static string connSql = ConfigurationManager.ConnectionStrings["connSql"].ConnectionString;
        static void Main(string[] args)
        {
            //DataTableUsing();
            //DataSetUsing();
            DataRealationsUsing();

        }
        /// <summary>
        /// 创建与数据库的连接，并查询数据库表的内容
        /// </summary>
        /// <returns>List<StudentModel>返回的是StudentModel类型的List集合</returns>
        private static List<StudentModel> connectedSql() {
            List<StudentModel> lstu = new List<StudentModel>();
            using (SqlConnection conn = new SqlConnection(connSql)) {
                string tSql = "select code,name,sex,address,age,classteacher from student";
                SqlCommand sc = new SqlCommand(tSql, conn);
                conn.Open();
                SqlDataReader sdr = sc.ExecuteReader(CommandBehavior.CloseConnection);
                
                if (sdr.HasRows)
                {
                    int indexId = sdr.GetOrdinal("code");//获取制定列名的列序号
                    int indexName = sdr.GetOrdinal("name");//获取制定列名的列序号
                    int indexSex = sdr.GetOrdinal("sex");//获取制定列名的列序号
                    int indexAddress = sdr.GetOrdinal("address");//获取制定列名的列序号
                    int indexAge = sdr.GetOrdinal("age");//获取制定列名的列序号
                    int indexTeacher = sdr.GetOrdinal("classteacher");//获取制定列名的列序号
                    while (sdr.Read())
                    {
                        StudentModel stu = new StudentModel();
                        stu.code = sdr.GetString(indexId);
                        stu.name = sdr.GetString(indexName);
                        stu.sex = sdr.GetInt32(indexSex);
                        stu.address = sdr.GetString(indexAddress);
                        stu.age = sdr.GetInt32(indexAge);
                        stu.teacher = sdr.GetString(indexTeacher);
                        lstu.Add(stu);
                    }
                }
                sdr.Close();
            }
            return lstu;
        }
        private static List<TeacherModel> connectedSq2()
        {
            List<TeacherModel> ltm = new List<TeacherModel>();
            using (SqlConnection conn = new SqlConnection(connSql))
            {
                string tSql = "select teacherId,teaname,teasex,teaheight,teaage,createtime from teacher";
                SqlCommand sc = new SqlCommand(tSql, conn);
                conn.Open();
                SqlDataReader sdr = sc.ExecuteReader(CommandBehavior.CloseConnection);

                if (sdr.HasRows)
                {
                    int indexId = sdr.GetOrdinal("teacherId");//获取制定列名的列序号
                    int indexName = sdr.GetOrdinal("teaname");//获取制定列名的列序号
                    int indexSex = sdr.GetOrdinal("teasex");//获取制定列名的列序号
                    int indexHeight = sdr.GetOrdinal("teaheight");//获取制定列名的列序号
                    int indexAge = sdr.GetOrdinal("teaage");//获取制定列名的列序号
                    int indexCreatetime = sdr.GetOrdinal("createtime");//获取制定列名的列序号
                    while (sdr.Read())
                    {
                        TeacherModel tm = new TeacherModel();
                        tm.teacherId = sdr.GetString(indexId);
                        tm.teaname = sdr.GetString(indexName);
                        tm.teasex = sdr.GetInt32(indexSex);
                        tm.teaheight = sdr.GetDecimal(indexHeight);
                        tm.teaage = sdr.GetInt32(indexAge);
                        tm.createtime = sdr.GetDateTime(indexCreatetime);
                        ltm.Add(tm);
                    }
                }
                sdr.Close();
            }
            return ltm;
        }
        /// <summary>
        /// 内存中对于表的创建及使用
        /// </summary>
        private static void DataTableUsing() {

                //1
                //DataTable dt = new DataTable();
                //dt.TableName = "StudentInfo";//定义内存中的表名
                List<StudentModel> lstu = connectedSql();
                //2
                DataTable dt1 = new DataTable("StudentInfo");
                DataColumn dc = new DataColumn();
                dc.ColumnName = "code";
                dc.DataType = typeof(string);//定义列类型
                dt1.Columns.Add(dc);//添加1列
                dt1.Columns.Add("name", typeof(string));//优先添加列的方式
                dt1.Columns.Add("sex", typeof(int));
                dt1.Columns.Add("address", typeof(string));
                dt1.Columns.Add("age", typeof(int));
                dt1.Columns.Add("teacher", typeof(string));
            dt1.PrimaryKey = new DataColumn[] { dt1.Columns[0] };//主键
                dt1.Constraints.Add(new UniqueConstraint(dt1.Columns[1]));//添加唯一性的约束可以为空，只能有一个

                DataRow dr;
                for (int i = 0; i < lstu.Count; i++)
                {
                    dr = dt1.NewRow();
                    dr["code"] = lstu[i].code;
                    dr["name"] = lstu[i].name;
                    dr["sex"] = lstu[i].sex;
                    dr["address"] = lstu[i].address;
                    dr["age"] = lstu[i].age;
                    dr["teacher"] = lstu[i].teacher;
                dt1.Rows.Add(dr);//状态为ADD
                    //dt1.RejectChanges();//回滚操作
                    //dt1.AcceptChanges();//提交更改 状态为unChanged
                    //dr["sex"] = 0;//修改 状态为modified 已修改
                    //dt1.AcceptChanges();//状态为unChanged
                    //dr.Delete();//状态为deleted 已删除
                    //dt1.AcceptChanges();//状态为detached
                    //dt1.Rows.Remove(dr); //状态为detached
                }
                //dt1.Clear();//清楚数据
                //DataTable dt2 = dt1.Copy();//复制结构和数据
                //DataTable dt3 = dt1.Clone();//复制结构
                //dt1.Merge(dt2);//融合表数据，相同数据会剔除
                DataRow[] rows = dt1.Select();
                DataRow[] rows1 = dt1.Select("age >10", "age desc");//增加条件
            
        }

        /// <summary>
        /// DataSet的使用 DataSet是内存中的缓存，可以在里面进行暂时存储或计算内存表的数据
        /// </summary>
        private static void DataSetUsing()
        {
            //1.
            //DataSet ds = new DataSet();
            //ds.DataSetName = "ds1";//设置内存的名称
            //2.
            DataSet ds = new DataSet("ds1");
            DataTable dt1 = new DataTable("dt1");
            //将内存中的表添加到ds1缓存中
            ds.Tables.Add(dt1);
            DataTable dt = ds.Tables[0];
            //ds.Relations.Add();添加关系
            //ds.AcceptChanges();
            //ds.RejectChanges();
            //ds.Clear();
            //ds.Copy();
            //ds.Clone();
            //ds.Merge();
            Console.ReadKey();
        }

        private static void InitData(DataTable dt) { 


        }

        private static void DataRealationsUsing() {

            //获取数据集合
            List<StudentModel> lsm = connectedSql();
            List<TeacherModel> ltm = connectedSq2();
            //关系 一对一 一个表的列数太多，拆分两个表
            //     一对多 
            //     多对多 中间表存储关系  权限分配
            DataSet ds = new DataSet();

            DataTable dt1 = new DataTable();

            DataTable dt2 = new DataTable();
            ds.Tables.Add(dt1);
            ds.Tables.Add(dt2);

            dt1.Columns.Add("code", typeof(string));//优先添加列的方式
            dt1.Columns.Add("name", typeof(string));//优先添加列的方式
            dt1.Columns.Add("sex", typeof(int));
            dt1.Columns.Add("address", typeof(string));
            dt1.Columns.Add("age", typeof(int));
            dt1.Columns.Add("teacher", typeof(string));
            dt1.PrimaryKey = new DataColumn[] { dt1.Columns[0] };//主键
            dt1.Constraints.Add(new UniqueConstraint(dt1.Columns[1]));//添加唯一性的约束可以为空，只能有一个
            for (int i = 0; i < lsm.Count; i++)
            {
                DataRow dr = dt1.NewRow();//在dt1表中创建行,然后从lsm数据集合中赋值给表的列
                dr["code"] = lsm[i].code;
                dr["name"] = lsm[i].name;
                dr["sex"] = lsm[i].sex;
                dr["address"] = lsm[i].address;
                dr["age"] = lsm[i].age;
                dr["teacher"] = lsm[i].teacher;
                dt1.Rows.Add(dr);
            }


            dt2.Columns.Add("teacherId", typeof(string));//优先添加列的方式
            dt2.Columns.Add("teaname", typeof(string));//优先添加列的方式
            dt2.Columns.Add("teasex", typeof(int));
            dt2.Columns.Add("teaheight", typeof(string));
            dt2.Columns.Add("teaage", typeof(int));
            dt2.Columns.Add("createtime", typeof(DateTime));
            dt2.PrimaryKey = new DataColumn[] { dt2.Columns[0] };//主键
            dt2.Constraints.Add(new UniqueConstraint(dt2.Columns[1]));//添加唯一性的约束可以为空，只能有一个


            

            for (int i = 0; i < ltm.Count; i++)
            {
                DataRow dr2 = dt2.NewRow();//在dt1表中创建行,然后从lsm数据集合中赋值给表的列
                dr2["teacherId"] = ltm[i].teacherId;
                dr2["teaname"] = ltm[i].teaname;
                dr2["teasex"] = ltm[i].teasex;
                dr2["teaheight"] = ltm[i].teaheight;
                dr2["teaage"] = ltm[i].teaage;
                dr2["createtime"] = ltm[i].createtime;
                dt2.Rows.Add(dr2);
            }
            //dt1.Constraints.Add(new ForeignKeyConstraint("fk", dt2.Columns["teacherId"], dt1.Columns["teacher"]));//给dt1添加外键约束
            //默认情况下，创建关系就会自动创建外键关系
            DataRelation drlt = new DataRelation("relation",dt2.Columns[0],dt1.Columns["teacher"]);
            ds.Relations.Add(drlt);

            foreach (DataRow dr in dt2.Rows)
            {
                DataRow[] r = dr.GetChildRows(drlt);
                foreach (DataRow drs in r)
                {
                    Console.WriteLine($"学号:{drs[0].ToString().Trim()}学生:{drs[1].ToString().Trim()},所属老师:{drs[5].ToString().Trim()},老师称呼:{dr[1].ToString().Trim()}");
                }
            }
            Console.ReadKey();
        }
    }
}

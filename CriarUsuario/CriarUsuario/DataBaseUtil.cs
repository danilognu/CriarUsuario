using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CriarUsuario
{
    class DataBaseUtil
    {

        public int InsertUser(Usuario usuario)
        {

            try
            {
                SqlConnection connection = new SqlConnection(ConnectionString.connectionString);
                int IdCurrent = Searchid();

                string insertSql = "INSERT INTO T019_USUARIO(" +
                                   " A019_cd_usuario" +
                                   " , A019_nome_usuario" +
                                   " , A019_login" +
                                   " , A019_senha" +
                                   ", A019_ind_desat" +
                                   ", A019_dta_ult_senha" +
                                   ", A019_ind_comiss" +
                                   ", A019_ind_auto_cadastro" +
                                   ", A019_dta_inclusao" +
                                   ", A019_usu_inclusao" +
                                   ", A019_dta_ult_alt" +
                                   ", A019_usu_ult_alt" +
                                   ", A019_ind_leitor_atualizacao" +
                                   ", A011_cd_empresa_lot" +
                                   ", A019_ind_orcamentista" +
                                   ", A019_ind_preposto" +
                                   ", A019_retira_verificacao_dat_os" +
                                   ", A019_permissao_desativacao" +
                                   ", A019_ind_usuario_portal" +
                                   ", A019_ind_considerar_cliente" +
                                   ") " +
                                   " VALUES(" +
                                   "   " + IdCurrent + "" +
                                   ", '" + usuario.Nome + "'" +
                                   ", '" + usuario.Login + "'" +
                                   ", 'ASfe@0#'" +
                                   ", 0" +
                                   ", GETDATE()" +
                                   ", 0" +
                                   ", 0" +
                                   ", GETDATE()" +
                                   ", 1" +
                                   ", GETDATE()" +
                                   ", 1" +
                                   ", 0" +
                                   ", 8" +
                                   ", 1" +
                                   ", 1" +
                                   ", 0" +
                                   ", 0" +
                                   ", 0" +
                                   ", 0" +
                                   ")";
                connection.Open();
                SqlCommand cmd = new SqlCommand(insertSql, connection);
                cmd.ExecuteReader();

                int IdGenerate = SearchIdCurrent();
                return IdGenerate;

            }
            catch (Exception e)
            {
                Console.WriteLine("Problema: " + e.Message);
                return 0;
            }

        }

        public int SearchIdCurrent()
        {
            SqlConnection connection = new SqlConnection(ConnectionString.connectionString);

            string sqlBusca = "SELECT MAX(A019_cd_usuario) FROM T019_USUARIO";
            connection.Open();

            SqlCommand command = new SqlCommand(sqlBusca, connection);
            SqlDataReader result = command.ExecuteReader();
            while (result.Read())
            {
                return int.Parse( result.GetValue(0).ToString() );
            }
            return 0;

        }

        public int Searchid()
        {
            SqlConnection connection = new SqlConnection(ConnectionString.connectionString);

            string sqlBuscaId = "SELECT MAX(A019_cd_usuario)+1 FROM T019_USUARIO";
            connection.Open();

            SqlCommand cmd = new SqlCommand(sqlBuscaId, connection);
            SqlDataReader resultSearch = cmd.ExecuteReader();
            while (resultSearch.Read())
            {
                return int.Parse(resultSearch.GetValue(0).ToString());
            }
            return 0;
        }

        public List<PerfilUsuario> SearchPermission(int idUsuario)
        {

            List<PerfilUsuario> perfils = new List<PerfilUsuario>();

            SqlConnection connection = new SqlConnection(ConnectionString.connectionString);
            string sqlBusca = "SELECT A019_cd_usuario,A024_cod_perfil FROM T026_PERFIL_USUARIO WHERE A019_cd_usuario = " + idUsuario;
            connection.Open();

            SqlCommand cmd = new SqlCommand(sqlBusca, connection);
            SqlDataReader resultSearch = cmd.ExecuteReader();
            while (resultSearch.Read())
            {
                PerfilUsuario perfil = new PerfilUsuario();

                perfil.CodigoUsuario = int.Parse(resultSearch.GetValue(0).ToString());
                perfil.CodigoPerfil = int.Parse(resultSearch.GetValue(1).ToString());
                perfils.Add(perfil);

            }
            return perfils;

        }

        public void InsertPermission(List<PerfilUsuario> perfils, int IdUserCopy)
        {

            SqlConnection connection = new SqlConnection(ConnectionString.connectionString);

            Console.WriteLine("Begin Copinando Perfil ---------");
            foreach (PerfilUsuario perfil in perfils)
            {
                string sqlInsert = "INSERT T026_PERFIL_USUARIO " +
                                   " (" +
                                   " A019_cd_usuario" +
                                   ",A024_cod_perfil" +
                                   ",A027_cod_site" +
                                   ",Dta_inc_alt" +
                                   ",Usu_inc_alt" +
                                   ") " +
                                   "VALUES (" +
                                   "" + IdUserCopy + "," +
                                   "" + perfil.CodigoPerfil + "," +
                                   "1," +
                                   "GETDATE()," +
                                   "1" +
                                   ")";

                connection.Open();
                SqlCommand cmd = new SqlCommand(sqlInsert, connection);
                cmd.ExecuteReader();
                connection.Close();
                Console.WriteLine(" Codigo Perfil:" + perfil.CodigoPerfil);
            }
            Console.WriteLine("Begin Copinando Perfil ---------");


        }

        public int SearchIdUserCopy(Usuario usuario)
        {
            SqlConnection connectionC = new SqlConnection(ConnectionString.connectionString);
            string sqlSearch = "SELECT TOP 1 A019_cd_usuario FROM T019_USUARIO WHERE A019_login = '" + usuario.LoginCopia + "'";
            connectionC.Open();

            SqlCommand cmdC = new SqlCommand(sqlSearch, connectionC);
            SqlDataReader resultSearchC = cmdC.ExecuteReader();
            while (resultSearchC.Read())
            {
                return int.Parse(resultSearchC.GetValue(0).ToString());
            }
            return 0;
        }

        public List<Empresa> SearchCompany()
        {
            Empresa empresa = null;
            List<Empresa> empresas = new List<Empresa>();

            SqlConnection connection = new SqlConnection(ConnectionString.connectionString);
            string sqlBusca = "SELECT A011_cd_empresa FROM T011_EMPRESA WHERE a011_in_ativo = 1";
            connection.Open();

            SqlCommand command = new SqlCommand(sqlBusca,connection);
            SqlDataReader result = command.ExecuteReader();
            while (result.Read())
            {
                empresa = new Empresa();
                empresa.CodigoEmpresa = int.Parse(result.GetValue(0).ToString());
                empresas.Add(empresa);

            }
            connection.Close();

            return empresas;
        }
        
        public void InsertCompanyUser(List<Empresa> empresas, int idUser)
        {

            SqlConnection connection = new SqlConnection(ConnectionString.connectionString);

            Console.WriteLine("Begin Cadastrando empresa ---------");
            foreach (Empresa empresa in empresas)
            {
                string sqlInsert = "INSERT INTO T071_usuario_empresa VALUES (" + empresa.CodigoEmpresa + "," + idUser + " )";
                connection.Open();
                SqlCommand command = new SqlCommand(sqlInsert, connection);
                command.ExecuteReader();
                connection.Close();
                Console.WriteLine(" Codigo empresa: " + empresa.CodigoEmpresa);
            }
            Console.WriteLine("End Cadastrando empresa ---------");


        }

        public void InsertAccessTime(int IdUser)
        {

            Console.WriteLine("Criando Horario Acesso ---------");
            SqlConnection connection = new SqlConnection(ConnectionString.connectionString);

            string sqlInsert = "INSERT INTO " +
                               " T072_PERFIL_HORARIO_USUARIO " +
                               "(A019_cd_usuario" +
                               ",A027_cod_site" +
                               ",A041_cod_perfil_horario" +
                               ",Dta_inc_alt" +
                               ",Usu_inc_alt) " +
                               " SELECT " +
                               "    "+ IdUser + "" +
                               "    ,1" +
                               "    ,A041_cod_perfil_horario" +
                               "    ,GETDATE()" +
                               "    ,1 " +
                               " FROM " +
                               " T041_PERFIL_HORARIO " +
                               " WHERE " +
                               "    A041_cod_perfil_horario IN(1,5) " +
                               "    AND A027_cod_site = 1";
            connection.Open();
            SqlCommand command = new SqlCommand(sqlInsert, connection);
            command.ExecuteReader();
            connection.Close();

        }


    }

}

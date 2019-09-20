using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CriarUsuario
{
    class Program
    {
        static void Main(string[] args)
        {

            Usuario usuario = new Usuario();

            Console.WriteLine("Digite Nome Novo Usuario:");
            usuario.Nome = Console.ReadLine();
            Console.WriteLine("Digite Login:");
            usuario.Login = Console.ReadLine();
            Console.WriteLine("Digite Login de copia perfil");
            usuario.LoginCopia = Console.ReadLine();

            bool _valida = false;
            if (usuario.Nome != "")
            {
                _valida = true;
            }else if(usuario.Login != "")
            {
                _valida = true;
            }else if (usuario.LoginCopia != "")
            {
                _valida = true;
            }

            if (_valida)
            {
                DataBaseUtil dataBaseUtil = new DataBaseUtil();
                int IdUsuario = dataBaseUtil.InsertUser(usuario);
                int IdUserCopy = dataBaseUtil.SearchIdUserCopy(usuario);

                List<PerfilUsuario> usuariosPerfil = dataBaseUtil.SearchPermission(IdUserCopy);
                dataBaseUtil.InsertPermission(usuariosPerfil, IdUsuario);

                List<Empresa> empresas = dataBaseUtil.SearchCompany();
                dataBaseUtil.InsertCompanyUser(empresas, IdUsuario);

                dataBaseUtil.InsertAccessTime(IdUsuario);

                Console.WriteLine("--------===============---------");
                Console.WriteLine("Usuario: " + usuario.Nome + " Criado");
                Console.WriteLine("Login: " + usuario.Login);
                Console.WriteLine("Senha: lets123" );
                Console.ReadKey();


            }
            else
            {
                Console.WriteLine("Digite todos as informações Pó, Programa será fechado!!, tente novamnete");
            }




        }
    }
}


using AppWebThais.Configs;
using AppWebThais.Model;
using MySql.Data.MySqlClient;

namespace AppWebThais.DAO
{
    public class ProcessoDAO
    {
        private readonly Conexao _conexao;

        // A Conexao é injetada pelo container de dependências
        public ProcessoDAO(Conexao conexao)
        {
            _conexao = conexao;
        }

        // READ — lista todos os processos
        public List<Processo> Listar()
        {
            var lista = new List<Processo>();

            using var con = _conexao.GetConnection();

            var comando = con.CreateCommand();
            comando.CommandText = "SELECT * FROM processos;";

            using var leitor = (MySqlDataReader)comando.ExecuteReader();

            while (leitor.Read())
            {
                lista.Add(MapearProcesso(leitor));
            }

            return lista;
        }

        // Método auxiliar: converte a linha atual do leitor em um objeto Processo
        private static Processo MapearProcesso(MySqlDataReader leitor)
        {
            return new Processo
            {
                Id = leitor.GetInt32("id_pro"),
                Numero = DAOHelper.GetString(leitor, "numero_pro"),
                Data = DAOHelper.GetDateOnly(leitor, "data_pro"),
                Interessado = DAOHelper.GetString(leitor, "interessado_pro"),
                Assunto = DAOHelper.GetString(leitor, "assunto_pro"),
                Descricao = DAOHelper.GetString(leitor, "descricao_pro"),
                Situacao = DAOHelper.GetString(leitor, "situacao_pro")
            };
        }

        // CREATE — insere um novo processo
        public void Inserir(Processo processo)
        {
            using var con = _conexao.GetConnection();

            string sql = @"INSERT INTO processos
                           (numero_pro, data_pro, interessado_pro,
                            assunto_pro, descricao_pro, situacao_pro)
                           VALUES
                           (@numero, @data, @interessado, @assunto,
                            @descricao, @situacao)";

            using var comando = con.CreateCommand();
            comando.CommandText = sql;

            comando.Parameters.AddWithValue("@numero", processo.Numero);

            comando.Parameters.AddWithValue("@data",
                processo.Data!.Value.ToDateTime(TimeOnly.MinValue));

            comando.Parameters.AddWithValue("@interessado", processo.Interessado);
            comando.Parameters.AddWithValue("@assunto", processo.Assunto);
            comando.Parameters.AddWithValue("@descricao", processo.Descricao);
            comando.Parameters.AddWithValue("@situacao", processo.Situacao);

            comando.ExecuteNonQuery();
        }

        // 1.1 — READ: busca um processo pelo ID
        public Processo? ObterPorId(int id)
        {
            using var con = _conexao.GetConnection();

            const string sql = @"SELECT id_pro, numero_pro, data_pro,
                                        interessado_pro, assunto_pro,
                                        descricao_pro, situacao_pro
                                 FROM processos
                                 WHERE id_pro = @id";

            using var comando = con.CreateCommand();
            comando.CommandText = sql;

            comando.Parameters.AddWithValue("@id", id);

            using var leitor = (MySqlDataReader)comando.ExecuteReader();

            if (!leitor.Read())
            {
                return null;
            }

            return MapearProcesso(leitor);
        }

        // 1.2 — UPDATE: atualiza um processo existente
        public void Atualizar(Processo processo)
        {
            using var con = _conexao.GetConnection();

            const string sql = @"UPDATE processos
                                 SET numero_pro = @numero,
                                     data_pro = @data,
                                     interessado_pro = @interessado,
                                     assunto_pro = @assunto,
                                     descricao_pro = @descricao,
                                     situacao_pro = @situacao
                                 WHERE id_pro = @id";

            using var comando = con.CreateCommand();
            comando.CommandText = sql;

            comando.Parameters.AddWithValue("@numero", processo.Numero);

            comando.Parameters.AddWithValue("@data",
                processo.Data!.Value.ToDateTime(TimeOnly.MinValue));

            comando.Parameters.AddWithValue("@interessado", processo.Interessado);
            comando.Parameters.AddWithValue("@assunto", processo.Assunto);
            comando.Parameters.AddWithValue("@descricao", processo.Descricao);
            comando.Parameters.AddWithValue("@situacao", processo.Situacao);
            comando.Parameters.AddWithValue("@id", processo.Id);

            comando.ExecuteNonQuery();
        }
        // DELETE — exclui um processo pelo ID
        public void Excluir(int id)
        {
            using var con = _conexao.GetConnection();

            const string sql = "DELETE FROM processos WHERE id_pro = @id";

            using var comando = con.CreateCommand();
            comando.CommandText = sql;

            comando.Parameters.AddWithValue("@id", id);

            comando.ExecuteNonQuery();
        }
    }
}

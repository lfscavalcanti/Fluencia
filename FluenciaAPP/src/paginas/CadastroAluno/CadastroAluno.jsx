import { useState } from "react";
import style from "./CadastroAluno.module.css";
import AlunoAPI from "../../services/alunoAPI";

export function CadastroAluno({ onAlunoCadastrado }) {
    const [nome, setNome] = useState('');
    const [email, setEmail] = useState('');
    const [carregando, setCarregando] = useState(false);
    const [erro, setErro] = useState('');

    async function handleSubmit(e) {
        e.preventDefault();
        setCarregando(true);
        setErro('');

        try {
            const dados = await AlunoAPI.cadastrarAsync(nome, email);
            onAlunoCadastrado(dados);
        } catch {
            setErro("Não foi possível conectar com o servidor. Verifique se a API está rodando.");
        } finally {
            setCarregando(false);
        }
    }

    return (
        <div className={style.conteudo}>
            <h2 className={style.titulo}>Bem-vindo ao Fluência</h2>
            <p className={style.subtitulo}>Preencha seus dados para iniciar o teste de nivelamento.</p>

            {erro && <p className={style.erro}>{erro}</p>}

            <form onSubmit={handleSubmit}>
                <div className={style.campo}>
                    <label>Nome:</label>
                    <input
                        type="text"
                        placeholder="Digite seu nome completo"
                        value={nome}
                        onChange={(e) => setNome(e.target.value)}
                        required
                    />
                </div>

                <div className={style.campo}>
                    <label>E-mail:</label>
                    <input
                        type="email"
                        placeholder="Digite seu e-mail"
                        value={email}
                        onChange={(e) => setEmail(e.target.value)}
                        required
                    />
                </div>

                <button type="submit" disabled={carregando} className={style.botao_iniciar}>
                    {carregando ? "Cadastrando..." : "Iniciar Teste"}
                </button>
            </form>
        </div>
    );
}

export default CadastroAluno;
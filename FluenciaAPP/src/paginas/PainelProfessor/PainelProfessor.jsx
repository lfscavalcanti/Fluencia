import { useState, useEffect } from "react";
import style from "./PainelProfessor.module.css";
import ProfessorAPI from "../../services/professorAPI";

export function PainelProfessor({ onVoltar }) {
    const [professor, setProfessor] = useState(null);
    const [codigo, setCodigo] = useState("");
    const [senha, setSenha] = useState("");
    const [novaSenha, setNovaSenha] = useState("");
    const [confirmarSenha, setConfirmarSenha] = useState("");
    const [telaTrocarSenha, setTelaTrocarSenha] = useState(false);

    const [abaAtiva, setAbaAtiva] = useState("alunos");
    const [alunos, setAlunos] = useState([]);
    const [alunoSelecionado, setAlunoSelecionado] = useState(null);
    const [resultadosAluno, setResultadosAluno] = useState([]);

    const [novaQuestao, setNovaQuestao] = useState({
        nivel: "A1",
        topico: "",
        enunciado: "",
        opcaoA: "",
        opcaoB: "",
        opcaoC: "",
        opcaoD: "",
        opcaoE: "Não sei responder",
        respostaCorreta: "B"
    });

    const [erro, setErro] = useState("");
    const [sucesso, setSucesso] = useState("");
    const [carregando, setCarregando] = useState(false);

    useEffect(() => {
        if (professor && !professor.primeiroAcesso) {
            carregarAlunos();
        }
    }, [professor]);

    async function carregarAlunos() {
        try {
            setCarregando(true);
            const dados = await ProfessorAPI.listarAlunosAsync();
            setAlunos(dados);
        } catch {
            setErro("Não foi possível carregar a lista de alunos.");
        } finally {
            setCarregando(false);
        }
    }

    async function handleLogin(e) {
        e.preventDefault();
        setErro("");
        setSucesso("");

        try {
            const dados = await ProfessorAPI.loginAsync(codigo, senha);
            setProfessor(dados);

            if (dados.primeiroAcesso) {
                setTelaTrocarSenha(true);
            }
        } catch {
            setErro("Código de acesso ou senha incorretos.");
        }
    }

    async function handleTrocarSenha(e) {
        e.preventDefault();
        setErro("");

        if (novaSenha.length !== 4) {
            setErro("A nova senha deve ter exatamente 4 dígitos numéricos.");
            return;
        }

        if (novaSenha !== confirmarSenha) {
            setErro("A confirmação de senha não confere.");
            return;
        }

        try {
            await ProfessorAPI.alterarSenhaAsync(professor.id, novaSenha);
            setSucesso("Senha alterada com sucesso! Bem-vindo(a) ao painel.");
            setTelaTrocarSenha(false);
            setProfessor({ ...professor, primeiroAcesso: false });
        } catch {
            setErro("Erro ao alterar senha. Tente novamente.");
        }
    }

    async function handleVerResultados(aluno) {
        setAlunoSelecionado(aluno);
        setResultadosAluno([]);
        setErro("");

        try {
            const dados = await ProfessorAPI.obterResultadosAlunoAsync(aluno.id);
            setResultadosAluno(dados);
        } catch {
            setErro("Não foi possível carregar o histórico de testes deste aluno.");
        }
    }

    async function handleSalvarQuestao(e) {
        e.preventDefault();
        setErro("");
        setSucesso("");

        try {
            await ProfessorAPI.cadastrarQuestaoAsync(novaQuestao);
            setSucesso("Questão cadastrada com sucesso na prova!");
            setNovaQuestao({
                nivel: "A1",
                topico: "",
                enunciado: "",
                opcaoA: "",
                opcaoB: "",
                opcaoC: "",
                opcaoD: "",
                opcaoE: "Não sei responder",
                respostaCorreta: "B"
            });
        } catch {
            setErro("Erro ao cadastrar a questão. Verifique todos os campos.");
        }
    }

    // Tela de Trocar Senha no 1º Acesso
    if (telaTrocarSenha) {
        return (
            <div className={style.box_login}>
                <h2 className={style.titulo}>Primeiro Acesso</h2>
                <p className={style.subtitulo}>Por segurança, cadastre sua nova senha de 4 dígitos.</p>

                {erro && <div className={style.erro}>{erro}</div>}

                <form onSubmit={handleTrocarSenha}>
                    <div className={style.campo_form}>
                        <label>Nova Senha (4 dígitos):</label>
                        <input
                            type="password"
                            maxLength="4"
                            placeholder="Ex: 2468"
                            value={novaSenha}
                            onChange={(e) => setNovaSenha(e.target.value)}
                            required
                        />
                    </div>

                    <div className={style.campo_form}>
                        <label>Confirmar Nova Senha:</label>
                        <input
                            type="password"
                            maxLength="4"
                            placeholder="Repita a senha"
                            value={confirmarSenha}
                            onChange={(e) => setConfirmarSenha(e.target.value)}
                            required
                        />
                    </div>

                    <button type="submit" className={style.btn_salvar_questao}>
                        Salvar Nova Senha e Acessar
                    </button>
                </form>
            </div>
        );
    }

    // Tela de Login do Professor
    if (!professor) {
        return (
            <div className={style.box_login}>
                <h2 className={style.titulo}>Área do Professor</h2>
                <p className={style.subtitulo}>Acesso exclusivo para docentes da Art Manha</p>

                {erro && <div className={style.erro}>{erro}</div>}

                <form onSubmit={handleLogin}>
                    <div className={style.campo_form}>
                        <label>Código de Acesso (4 dígitos):</label>
                        <input
                            type="text"
                            maxLength="4"
                            placeholder="Ex: 0001"
                            value={codigo}
                            onChange={(e) => setCodigo(e.target.value)}
                            required
                        />
                    </div>

                    <div className={style.campo_form}>
                        <label>Senha:</label>
                        <input
                            type="password"
                            maxLength="10"
                            placeholder="Digite sua senha"
                            value={senha}
                            onChange={(e) => setSenha(e.target.value)}
                            required
                        />
                    </div>

                    <button type="submit" className={style.btn_salvar_questao}>
                        Entrar no Painel
                    </button>

                    <button type="button" onClick={onVoltar} className={style.btn_sair}>
                        Voltar para Área do Aluno
                    </button>
                </form>
            </div>
        );
    }

    // Painel Principal do Professor (Logado)
    return (
        <div className={style.container}>
            <div className={style.cabecalho}>
                <div>
                    <h2 className={style.titulo}>Painel do Docente</h2>
                    <p className={style.subtitulo}>Professor: {professor.nome} (Código: {professor.codigo})</p>
                </div>
                <button type="button" onClick={() => setProfessor(null)} className={style.btn_ver_detalhes}>
                    Sair
                </button>
            </div>

            <div className={style.nav_abas}>
                <button
                    type="button"
                    className={`${style.aba} ${abaAtiva === "alunos" ? style.aba_ativa : ""}`}
                    onClick={() => { setAbaAtiva("alunos"); setAlunoSelecionado(null); }}
                >
                    Alunos e Resultados
                </button>
                <button
                    type="button"
                    className={`${style.aba} ${abaAtiva === "questoes" ? style.aba_ativa : ""}`}
                    onClick={() => setAbaAtiva("questoes")}
                >
                    Cadastrar Nova Questão
                </button>
            </div>

            {erro && <div className={style.erro}>{erro}</div>}
            {sucesso && <div className={style.sucesso}>{sucesso}</div>}

            {abaAtiva === "alunos" && (
                <div>
                    <h3>Alunos Cadastrados</h3>
                    {carregando ? (
                        <p>Carregando alunos...</p>
                    ) : alunos.length === 0 ? (
                        <p>Nenhum aluno cadastrado até o momento.</p>
                    ) : (
                        <table className={style.tabela}>
                            <thead>
                                <tr>
                                    <th>ID</th>
                                    <th>Nome</th>
                                    <th>E-mail</th>
                                    <th>Data Cadastro</th>
                                    <th>Ações</th>
                                </tr>
                            </thead>
                            <tbody>
                                {alunos.map((a) => (
                                    <tr key={a.id}>
                                        <td>#{a.id}</td>
                                        <td><strong>{a.nome}</strong></td>
                                        <td>{a.email}</td>
                                        <td>{new Date(a.dataCadastro).toLocaleDateString()}</td>
                                        <td>
                                            <button
                                                type="button"
                                                onClick={() => handleVerResultados(a)}
                                                className={style.btn_ver_detalhes}
                                            >
                                                Ver Resultados
                                            </button>
                                        </td>
                                    </tr>
                                ))}
                            </tbody>
                        </table>
                    )}

                    {alunoSelecionado && (
                        <div className={style.card_detalhes}>
                            <h3>Histórico de Testes: {alunoSelecionado.nome}</h3>
                            {resultadosAluno.length === 0 ? (
                                <p>Este aluno ainda não concluiu nenhum teste de nivelamento.</p>
                            ) : (
                                resultadosAluno.map((res) => (
                                    <div key={res.id} className={style.item_resultado}>
                                        <div className={style.cabecalho_resultado}>
                                            <span className={style.badge_nivel}>Nível: {res.nivelIdentificado}</span>
                                            <span className={style.data_resultado}>
                                                {new Date(res.dataAvaliacao).toLocaleString()}
                                            </span>
                                        </div>
                                        <div className={style.linha_topico}>
                                            <strong>Tópicos Dominados:</strong> {res.topicosDominados || "Nenhum"}
                                        </div>
                                        <div className={style.linha_topico}>
                                            <strong>Tópicos a Melhorar:</strong> {res.topicosPendentes || "Nenhum"}
                                        </div>
                                        <div className={style.plano_texto}>
                                            <strong>Plano de Estudos Gerado pela IA:</strong>
                                            <p>{res.planoEstudos}</p>
                                        </div>
                                    </div>
                                ))
                            )}
                        </div>
                    )}
                </div>
            )}

            {abaAtiva === "questoes" && (
                <form onSubmit={handleSalvarQuestao}>
                    <h3>Nova Questão para a Prova</h3>

                    <div className={style.linha_dupla}>
                        <div className={style.campo_form}>
                            <label>Nível (CEFR):</label>
                            <select
                                value={novaQuestao.nivel}
                                onChange={(e) => setNovaQuestao({ ...novaQuestao, nivel: e.target.value })}
                            >
                                <option value="A1">A1 - Iniciante</option>
                                <option value="A2">A2 - Básico</option>
                                <option value="B1">B1 - Intermediário</option>
                                <option value="B2">B2 - Intermediário Superior</option>
                                <option value="C1">C1 - Avançado</option>
                                <option value="C2">C2 - Proficiente</option>
                            </select>
                        </div>

                        <div className={style.campo_form}>
                            <label>Tópico Gramatical:</label>
                            <input
                                type="text"
                                placeholder="Ex: Verb To Be, Simple Past..."
                                value={novaQuestao.topico}
                                onChange={(e) => setNovaQuestao({ ...novaQuestao, topico: e.target.value })}
                                required
                            />
                        </div>
                    </div>

                    <div className={style.campo_form}>
                        <label>Enunciado da Questão:</label>
                        <textarea
                            rows="3"
                            placeholder="Ex: My sister ___ a teacher at a music school."
                            value={novaQuestao.enunciado}
                            onChange={(e) => setNovaQuestao({ ...novaQuestao, enunciado: e.target.value })}
                            required
                        />
                    </div>

                    <div className={style.linha_dupla}>
                        <div className={style.campo_form}>
                            <label>Opção A:</label>
                            <input
                                type="text"
                                value={novaQuestao.opcaoA}
                                onChange={(e) => setNovaQuestao({ ...novaQuestao, opcaoA: e.target.value })}
                                required
                            />
                        </div>

                        <div className={style.campo_form}>
                            <label>Opção B:</label>
                            <input
                                type="text"
                                value={novaQuestao.opcaoB}
                                onChange={(e) => setNovaQuestao({ ...novaQuestao, opcaoB: e.target.value })}
                                required
                            />
                        </div>
                    </div>

                    <div className={style.linha_dupla}>
                        <div className={style.campo_form}>
                            <label>Opção C:</label>
                            <input
                                type="text"
                                value={novaQuestao.opcaoC}
                                onChange={(e) => setNovaQuestao({ ...novaQuestao, opcaoC: e.target.value })}
                                required
                            />
                        </div>

                        <div className={style.campo_form}>
                            <label>Opção D:</label>
                            <input
                                type="text"
                                value={novaQuestao.opcaoD}
                                onChange={(e) => setNovaQuestao({ ...novaQuestao, opcaoD: e.target.value })}
                                required
                            />
                        </div>
                    </div>

                    <div className={style.linha_dupla}>
                        <div className={style.campo_form}>
                            <label>Opção E (Elimina o Chute):</label>
                            <input
                                type="text"
                                value={novaQuestao.opcaoE}
                                onChange={(e) => setNovaQuestao({ ...novaQuestao, opcaoE: e.target.value })}
                                required
                            />
                        </div>

                        <div className={style.campo_form}>
                            <label>Alternativa Correta (Gabarito):</label>
                            <select
                                value={novaQuestao.respostaCorreta}
                                onChange={(e) => setNovaQuestao({ ...novaQuestao, respostaCorreta: e.target.value })}
                            >
                                <option value="A">Opção A</option>
                                <option value="B">Opção B</option>
                                <option value="C">Opção C</option>
                                <option value="D">Opção D</option>
                            </select>
                        </div>
                    </div>

                    <button type="submit" className={style.btn_salvar_questao}>
                        Cadastrar Questão na Prova
                    </button>
                </form>
            )}
        </div>
    );
}

export default PainelProfessor;
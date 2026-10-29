import { useState, useEffect } from "react";
import style from "./Prova.module.css";
import TesteAPI from "../../services/testeAPI";

export function Prova({ aluno, onFinalizarProva }) {
    const [questoes, setQuestoes] = useState([]);
    const [indiceAtual, setIndiceAtual] = useState(0);
    const [respostas, setRespostas] = useState([]);
    const [opcaoSelecionada, setOpcaoSelecionada] = useState('');
    const [carregando, setCarregando] = useState(true);
    const [enviando, setEnviando] = useState(false);
    const [erro, setErro] = useState('');

    useEffect(() => {
        async function carregarQuestoes() {
            try {
                const dados = await TesteAPI.obterQuestoesAsync();
                setQuestoes(dados);
            } catch {
                setErro("Não foi possível carregar a prova. Verifique se a API está rodando.");
            } finally {
                setCarregando(false);
            }
        }
        carregarQuestoes();
    }, []);

    function handleProxima() {
        if (!opcaoSelecionada) {
            alert("Por favor, selecione uma opção antes de continuar.");
            return;
        }

        const questaoAtual = questoes[indiceAtual];
        const novasRespostas = [
            ...respostas,
            {
                questaoId: questaoAtual.id,
                opcaoEscolhida: opcaoSelecionada
            }
        ];

        setRespostas(novasRespostas);
        setOpcaoSelecionada('');

        if (indiceAtual + 1 < questoes.length) {
            setIndiceAtual(indiceAtual + 1);
        } else {
            submeterProva(novasRespostas);
        }
    }

    async function submeterProva(respostasFinais) {
        setEnviando(true);
        setErro('');

        try {
            const resultado = await TesteAPI.responderAsync(aluno.id, respostasFinais);
            onFinalizarProva(resultado);
        } catch {
            setErro("Falha ao processar as respostas da prova.");
        } finally {
            setEnviando(false);
        }
    }

    if (carregando) return <p className={style.status_mensagem}>Carregando questões da prova...</p>;
    if (enviando) return <p className={style.status_mensagem}>Avaliando nível e gerando plano de estudos com IA...</p>;
    if (erro) return <p className={`${style.status_mensagem} ${style.erro_mensagem}`}>{erro}</p>;
    if (questoes.length === 0) return <p className={style.status_mensagem}>Nenhuma questão cadastrada no momento.</p>;

    const questao = questoes[indiceAtual];
    const opcoes = [
        { letra: 'A', texto: questao.opcaoA },
        { letra: 'B', texto: questao.opcaoB },
        { letra: 'C', texto: questao.opcaoC },
        { letra: 'D', texto: questao.opcaoD },
        { letra: 'E', texto: questao.opcaoE || 'Não sei responder' }
    ];

    return (
        <div className={style.card_prova}>
            <div className={style.cabecalho}>
                <span>Aluno: <strong>{aluno.nome}</strong></span>
                <span>Questão {indiceAtual + 1} de {questoes.length}</span>
            </div>

            <div className={style.caixa_questao}>
                <p className={style.subtitulo}>Tópico: {questao.topico} ({questao.nivel})</p>
                <h3 className={style.enunciado}>{questao.enunciado}</h3>
            </div>

            <div className={style.lista_opcoes}>
                {opcoes.map((op) => (
                    <label
                        key={op.letra}
                        className={`${style.item_opcao} ${op.letra === 'E' ? style.item_opcao_e : ''} ${opcaoSelecionada === op.letra ? style.item_opcao_selecionada : ''}`}
                    >
                        <input
                            type="radio"
                            name="opcao"
                            value={op.letra}
                            checked={opcaoSelecionada === op.letra}
                            onChange={() => setOpcaoSelecionada(op.letra)}
                        />
                        <strong>{op.letra})</strong>&nbsp;{op.texto}
                    </label>
                ))}
            </div>

            <button type="button" onClick={handleProxima} className={style.botao_acao}>
                {indiceAtual + 1 === questoes.length ? "Finalizar Prova" : "Próxima Questão"}
            </button>
        </div>
    );
}

export default Prova;
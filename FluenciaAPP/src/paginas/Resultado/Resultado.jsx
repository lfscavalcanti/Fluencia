import style from "./Resultado.module.css";

export function Resultado({ resultado, onReiniciar }) {
    let dominados = [];
    let pendentes = [];

    try {
        dominados = Array.isArray(resultado.topicosDominados)
            ? resultado.topicosDominados
            : JSON.parse(resultado.topicosDominados || "[]");
    } catch {
        dominados = [];
    }

    try {
        pendentes = Array.isArray(resultado.topicosPendentes)
            ? resultado.topicosPendentes
            : JSON.parse(resultado.topicosPendentes || "[]");
    } catch {
        pendentes = [];
    }

    
    function handleSalvarPDF() {
        window.print();
    }

    return (
        <div className={style.card_resultado}>
            <div className={style.cabecalho_aluno}>
                <div>
                    <h2 className={style.nome_aluno}>{resultado.nomeAluno || "Aluno"}</h2>
                    <p className={style.detalhes_aluno}>
                        📅 {new Date(resultado.dataAvaliacao).toLocaleDateString()} | Sistema de Nivelamento Art Manha
                    </p>
                </div>
                <div className={style.badge_nivel}>
                    <h3>{resultado.nivelIdentificado}</h3>
                    <span>Nível</span>
                </div>
            </div>

            <div className={style.grid_topicos}>
                <div className={style.painel_topicos}>
                    <h4>✓ Tópicos Dominados</h4>
                    {dominados.length > 0 ? (
                        dominados.map((topico, idx) => (
                            <div key={idx} className={style.item_topico_verde}>
                                <span>{topico}</span>
                                <strong>100%</strong>
                            </div>
                        ))
                    ) : (
                        <p style={{ fontSize: "13px", color: "#888" }}>Nenhum tópico dominado.</p>
                    )}
                </div>

                <div className={style.painel_topicos}>
                    <h4>⚠ Tópicos a Melhorar</h4>
                    {pendentes.length > 0 ? (
                        pendentes.map((topico, idx) => (
                            <div key={idx} className={style.item_topico_laranja}>
                                <span>{topico}</span>
                                <strong>Reforçar</strong>
                            </div>
                        ))
                    ) : (
                        <p style={{ fontSize: "13px", color: "#137333" }}>Nenhuma pendência encontrada.</p>
                    )}
                </div>
            </div>

            <div className={style.caixa_plano_vinho}>
                <h4>✨ Plano de Estudos Personalizado (IA)</h4>
                <div className={style.texto_plano}>
                    {resultado.planoEstudos}
                </div>
            </div>

            <div className={`${style.botoes_container} sem-impressao`}>
                <button type="button" onClick={handleSalvarPDF} className={style.botao_pdf}>
                    📄 Salvar como PDF / Imprimir
                </button>
                <button type="button" onClick={onReiniciar} className={style.botao_reiniciar}>
                    Realizar Novo Teste
                </button>
            </div>
        </div>
    );
}

export default Resultado;
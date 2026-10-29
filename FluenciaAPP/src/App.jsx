import { useState } from "react";
import CadastroAluno from "./paginas/CadastroAluno/CadastroAluno";
import Prova from "./paginas/Prova/Prova";
import Resultado from "./paginas/Resultado/Resultado";

function App() {
  const [aluno, setAluno] = useState(null);
  const [resultado, setResultado] = useState(null);

  // Chamado quando o aluno termina o cadastro
  function handleAlunoCadastrado(alunoCadastrado) {
    setAluno(alunoCadastrado);
  }

  // Chamado quando a prova é concluída e a API devolve o resultado
  function handleFinalizarProva(resultadoFinal) {
    setResultado(resultadoFinal);
  }

  // Chamado para reiniciar o fluxo
  function handleReiniciar() {
    setAluno(null);
    setResultado(null);
  }

  return (
    <div>
      {!aluno && (
        <CadastroAluno onAlunoCadastrado={handleAlunoCadastrado} />
      )}

      {aluno && !resultado && (
        <Prova aluno={aluno} onFinalizarProva={handleFinalizarProva} />
      )}

      {resultado && (
        <Resultado resultado={resultado} onReiniciar={handleReiniciar} />
      )}
    </div>
  );
}

export default App;
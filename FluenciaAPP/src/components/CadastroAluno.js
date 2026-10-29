import React, { useState } from 'react';

function CadastroAluno({ onAlunoCadastrado }) {
  const [nome, setNome] = useState('');
  const [email, setEmail] = useState('');
  const [carregando, setCarregando] = useState(false);
  const [erro, setErro] = useState('');

  const handleSubmit = async (e) => {
    e.preventDefault();
    setCarregando(true);
    setErro('');

    try {
      const resposta = await fetch('https://localhost:7001/api/Aluno/cadastrar', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify({ nome: nome, email: email })
      });

      if (!resposta.ok) {
        throw new Error('Erro ao cadastrar aluno.');
      }

      const dados = await resposta.json();
      onAlunoCadastrado(dados);
    } catch (err) {
      setErro('Não foi possível conectar com o servidor. Verifique se a API está rodando.');
    } finally {
      setCarregando(false);
    }
  };

  return (
    <div style={{ maxWidth: '400px', margin: '50px auto', padding: '20px', border: '1px solid #ccc', borderRadius: '8px', fontFamily: 'Arial, sans-serif' }}>
      <h2>Bem-vindo ao Fluência</h2>
      <p>Preencha seus dados para iniciar o teste de nivelamento de inglês.</p>

      {erro && <p style={{ color: 'red' }}>{erro}</p>}

      <form onSubmit={handleSubmit}>
        <div style={{ marginBottom: '15px' }}>
          <label style={{ display: 'block', marginBottom: '5px' }}>Nome:</label>
          <input
            type="text"
            required
            value={nome}
            onChange={(e) => setNome(e.target.value)}
            style={{ width: '100%', padding: '8px', boxSizing: 'border-box' }}
            placeholder="Seu nome completo"
          />
        </div>

        <div style={{ marginBottom: '15px' }}>
          <label style={{ display: 'block', marginBottom: '5px' }}>E-mail:</label>
          <input
            type="email"
            required
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            style={{ width: '100%', padding: '8px', boxSizing: 'border-box' }}
            placeholder="seu.email@exemplo.com"
          />
        </div>

        <button
          type="submit"
          disabled={carregando}
          style={{ width: '100%', padding: '10px', backgroundColor: '#007bff', color: '#fff', border: 'none', borderRadius: '4px', cursor: 'pointer', fontWeight: 'bold' }}
        >
          {carregando ? 'Cadastrando...' : 'Iniciar Teste'}
        </button>
      </form>
    </div>
  );
}

export default CadastroAluno;
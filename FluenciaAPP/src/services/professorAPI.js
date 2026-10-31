import { HTTPClient } from "./client";

const ProfessorAPI = {
    async loginAsync(codigo, senha) {
        try {
            const response = await HTTPClient.post("/api/Professor/login", {
                codigo: codigo,
                senha: senha
            });
            return response.data;
        } catch (error) {
            console.error("Erro ao realizar login do professor:", error);
            throw error;
        }
    },

    async alterarSenhaAsync(id, novaSenha) {
        try {
            const response = await HTTPClient.put("/api/Professor/alterar-senha", {
                id: id,
                novaSenha: novaSenha
            });
            return response.data;
        } catch (error) {
            console.error("Erro ao alterar senha do professor:", error);
            throw error;
        }
    },

    async listarAlunosAsync() {
        try {
            const response = await HTTPClient.get("/api/Aluno/listar");
            return response.data;
        } catch (error) {
            console.error("Erro ao listar alunos:", error);
            throw error;
        }
    },

    async obterResultadosAlunoAsync(alunoId) {
        try {
            const response = await HTTPClient.get(`/api/Resultado/aluno/${alunoId}`);
            return response.data;
        } catch (error) {
            console.error("Erro ao buscar resultados do aluno:", error);
            throw error;
        }
    },

    async cadastrarQuestaoAsync(questao) {
        try {
            const response = await HTTPClient.post("/api/Questao/cadastrar", questao);
            return response.data;
        } catch (error) {
            console.error("Erro ao cadastrar questao:", error);
            throw error;
        }
    }
};

export default ProfessorAPI;
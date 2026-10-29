import { HTTPClient } from "./client";

const AlunoAPI = {
    async cadastrarAsync(nome, email) {
        try {
            const response = await HTTPClient.post("/api/Aluno/cadastrar", {
                nome: nome,
                email: email
            });
            return response.data;
        } catch (error) {
            console.error("Erro ao cadastrar aluno:", error);
            throw error;
        }
    }
};

export default AlunoAPI;
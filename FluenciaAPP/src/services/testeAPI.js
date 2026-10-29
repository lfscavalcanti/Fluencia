import { HTTPClient } from "./client";

const TesteAPI = {
    async obterQuestoesAsync() {
        try {
            const response = await HTTPClient.get("/api/Teste/questoes");
            return response.data;
        } catch (error) {
            console.error("Erro ao obter questões:", error);
            throw error;
        }
    },

    async responderAsync(alunoId, respostas) {
        try {

            const response = await HTTPClient.post(`/api/Teste/responder/${alunoId}`, respostas);
            return response.data;
        } catch (error) {
            console.error("Erro ao enviar respostas:", error);
            throw error;
        }
    }
};

export default TesteAPI;
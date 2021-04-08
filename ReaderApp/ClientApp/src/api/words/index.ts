import { get, post } from 'services/HttpService';

export const getUnknownWords = async (fileId: string) => {
    const { data: words } = await get<string[]>(`/api/Words/unknown?fileId=${fileId}`);

    return words;
};

export const saveLearned = (learnedWords: string[]) => post('/api/Words/learned', learnedWords);

export const getInfoAboutWord = (word: string) =>
    get<{ definitions: string[]; lemma: string; translations: string[] }>(
        `/api/Words/${word}/info`
    );

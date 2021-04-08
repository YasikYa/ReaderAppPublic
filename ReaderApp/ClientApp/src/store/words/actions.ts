import { createAsyncThunk } from '@reduxjs/toolkit';
import { postTextFile } from 'api/files';
import { getInfoAboutWord, getUnknownWords, saveLearned } from 'api/words';
import { RootState } from 'store';
import { addFile } from 'store/files';

export const fetchUploadFile = createAsyncThunk(
    'words/fetchUploadFile',
    async (file: File, { dispatch }) => {
        const { unknownWords, fileInfo } = await postTextFile(file);
        dispatch(addFile(fileInfo));

        return { unknownWords, fileInfo };
    }
);

export const fetchLoadFileUnknownWords = createAsyncThunk(
    'words/fetchFileUnknownWords',
    async (fileId: string) => {
        const unknownWords = await getUnknownWords(fileId);

        return unknownWords;
    }
);

export const fetchSaveLearnedWords = createAsyncThunk<void, void, { state: RootState }>(
    'words/fetchSaveLearnedWords',
    async (_, { getState }) => {
        const state = getState();
        await saveLearned(state.words.learnedSession);
    }
);

export const fetchInfoAboutWord = createAsyncThunk(
    'words/fetchInfoAboutWord',
    async (word: string) => {
        const { data } = await getInfoAboutWord(word);
        return data;
    }
);

import { WordsStore } from './types';
import { createSlice, PayloadAction } from '@reduxjs/toolkit';
import { UserFileInfo } from 'api/files/types';
import { fetchLoadFileUnknownWords, fetchSaveLearnedWords, fetchUploadFile } from './actions';

const initialState: WordsStore = {
    workingFile: null,
    learnedSession: [],
    unknownWords: [],
};

export const wordsSlice = createSlice({
    name: 'words',
    initialState,
    reducers: {
        addToLearned: (state, action: PayloadAction<string>) => {
            const { unknownWords, learnedSession } = state;
            const { payload } = action;

            const wordIndex = unknownWords.indexOf(payload);
            if (wordIndex !== -1) {
                learnedSession.push(unknownWords[wordIndex]);
                unknownWords.splice(wordIndex, 1);
            }
        },
        setReadFile: (state, action: PayloadAction<UserFileInfo>) => {
            const { payload } = action;
            state.workingFile = { fileInfo: payload, isUploaded: false };
        },
        removeWorkingFile: (state, action: PayloadAction<string>) => {
            if (state.workingFile?.fileInfo.id === action.payload) {
                state.workingFile = null;
            }
        },
    },
    extraReducers: (builder) => {
        builder.addCase(
            fetchUploadFile.fulfilled,
            (state, { payload: { unknownWords, fileInfo } }) => {
                state.workingFile = { fileInfo, isUploaded: true };
                state.unknownWords = unknownWords.filter(
                    (word) => !state.learnedSession.includes(word)
                );
            }
        );
        builder.addCase(fetchLoadFileUnknownWords.fulfilled, (state, { payload }) => {
            state.unknownWords = payload.filter((word) => !state.learnedSession.includes(word));
        });
        builder.addCase(fetchSaveLearnedWords.fulfilled, (state) => {
            state.learnedSession = [];
        });
    },
});

export const { addToLearned, setReadFile, removeWorkingFile } = wordsSlice.actions;
export const wordsReduser = wordsSlice.reducer;

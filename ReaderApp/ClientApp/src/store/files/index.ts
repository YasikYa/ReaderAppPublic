import { createSlice, PayloadAction } from '@reduxjs/toolkit';
import { UserFileInfo } from 'api/files/types';
import { fetchFiles, fetchRemoveFile } from './actions';
import { FilesStore } from './types';

const initialState: FilesStore = {
    userFiles: [],
};

const filesSlice = createSlice({
    name: 'files',
    initialState,
    reducers: {
        addFile: (state, action: PayloadAction<UserFileInfo>) => {
            state.userFiles.push(action.payload);
        },
        addFiles: (state, action: PayloadAction<UserFileInfo[]>) => {
            state.userFiles = state.userFiles.concat(action.payload);
        },
    },
    extraReducers: (builder) => {
        builder.addCase(fetchFiles.fulfilled, (state, { payload: userFiles }) => {
            state.userFiles = userFiles;
        });
        builder.addCase(fetchRemoveFile.fulfilled, (state, { payload: fileId }) => {
            state.userFiles = state.userFiles.filter((userFile) => userFile.id != fileId);
        });
    },
});

export const { addFile, addFiles } = filesSlice.actions;
export const filesReducer = filesSlice.reducer;

import { createAsyncThunk } from '@reduxjs/toolkit';
import { deleteFile, getUserFiles } from 'api/files';
import { removeWorkingFile } from 'store/words';

export const fetchFiles = createAsyncThunk('files/fetchFiles', async (userId: string) => {
    const { data: userFiles } = await getUserFiles(userId);

    return userFiles;
});

export const fetchRemoveFile = createAsyncThunk(
    'files/fetchRemoveFile',
    async (fileId: string, { dispatch }) => {
        await deleteFile(fileId);

        dispatch(removeWorkingFile(fileId));

        return fileId;
    }
);

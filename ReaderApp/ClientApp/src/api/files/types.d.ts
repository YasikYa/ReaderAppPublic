export interface UserFileInfo {
    id: string;
    fileName: string
}

export interface PostFileResponse {
    unknownWords: string[],
    fileInfo: UserFileInfo
}
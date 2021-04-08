import { UserFileInfo } from "api/files/types";

export interface WorkingFile {
    fileInfo: UserFileInfo,
    isUploaded: boolean
}

export type WordsStore = {
    workingFile: WorkingFile | null
    unknownWords: string[],
    learnedSession: string[]
}
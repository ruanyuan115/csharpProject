#encoding=utf-8
#读取词典模块
import xlrd
class read_file_part:
    ##大连理工情感词典里的情感词类型简写
    positive = ['PA', 'PE', 'PD', 'PH', 'PG', 'PB', 'PK']
    negative = ['NA', 'NB', 'NJ', 'NH', 'PF', 'NI', 'NC', 'NG', 'NE', 'ND', 'NN', 'NK', 'NL', 'PC']

    extreme = ""            ##极其|extreme / 最|most           2
    very    = ""            ##很|very                          1.25
    more    = ""            ##较|more                          1.2
    _ish    = ""            ##稍|-ish                          0.8
    insufficiently = ""     ##欠|insufficiently                0.5
    over    = ""            ##超|over                          1.5
    noword  = ""            ##否定词
    dligemotion = []        ##大连理工情感词
    emotionnum   = 0        ##情感词总数
    hownetposemotion = []   ##知网hownet正向情感词
    hownetnegemotion = []   ##知网hownet负向情感词
    tsingposemotion = []    ##TSing正向情感词
    tsingnegemotion = []    ##TSing负向情感词

    ##路径
    HownetPath = 'NLP_test/SentimentAnalysisDic'+u'/知网Hownet情感词典'
    HownetPosEmoFile = u'/正面情感词语（中文）' + '.txt'
    HownetPosEvaFile = u'/正面评价词语（中文）' + '.txt'
    HownetNegEmoFile = u'/负面情感词语（中文）' + '.txt'
    HownetNegEvaFile = u'/负面评价词语（中文）' + '.txt'
    NegatePath = 'NLP_test/SentimentAnalysisDic' + u'/否定词典' + u'/否定' + '.txt'
    DLLGPath = 'NLP_test/SentimentAnalysisDic' + u'/大连理工情感词汇本体' + u'/情感词汇本体' + '.xlsx'
    TsinghuaPosFile = 'NLP_test/SentimentAnalysisDic' + u'/清华大学李军中文褒贬义词典' + '/tsinghua.positive.gb.txt'
    TsinghuaNegFile = 'NLP_test/SentimentAnalysisDic' + u'/清华大学李军中文褒贬义词典' + '/tsinghua.negative.gb.txt'

    ##读取文件函数
    def _read_file_(self, filename):
        file_object = open(filename)
        try:
            word = file_object.read().split()
        finally:
            file_object.close()
        return word

    ##读取大连理工情感词典
    def _read_dllg_emotion_file(self):
        # 打开文件
        bk = xlrd.open_workbook(self.DLLGPath)
        # 打开工作表
        sh = bk.sheet_by_name("Sheet1")
        # 获取行数
        self.emotionnum = sh.nrows-1
        word = [[] for i in range(1, sh.nrows)]
        for i in range(1, sh.nrows):
            word[i-1] = sh.row_values(i)
        return word

    ##读取知网Hownet情感词典
    def _read_hownet_emotion_file(self, filename1, filename2):
        word = []
        file_object1 = open(filename1)
        file_object2 = open(filename2)
        for line in file_object1:
            line = line.strip()
            word.append(line)
        for line in file_object2:
            line = line.strip()
            word.append(line)
        file_object1.close()
        file_object2.close()
        return word

    ##读取清华大学李中情感词典
    def _read_ts_emotion_file(self, filename1):
        word = []
        file_object1 = open(filename1)
        for line in file_object1:
            line = line.strip()
            word.append(line)
        file_object1.close()
        return word

    def __init__(self):
        self.extreme = self._read_file_(self.HownetPath +'/extreme.txt')
        self.very = self._read_file_(self.HownetPath +'/very.txt')
        self.more = self._read_file_(self.HownetPath +'/more.txt')
        self._ish = self._read_file_(self.HownetPath +'/-ish.txt')
        self.insufficiently = self._read_file_(self.HownetPath +'/insufficiently.txt')
        self.over = self._read_file_(self.HownetPath +'/over.txt')
        self.noword = self._read_file_(self.NegatePath)
        self.dligemotion = self._read_dllg_emotion_file()
        self.hownetposemotion = self._read_hownet_emotion_file(self.HownetPath + self.HownetPosEmoFile, self.HownetPath + self.HownetPosEvaFile)
        self.hownetnegemotion = self._read_hownet_emotion_file(self.HownetPath + self.HownetNegEmoFile, self.HownetPath + self.HownetNegEvaFile)
        self.tsingposemotion = self._read_ts_emotion_file(self.TsinghuaPosFile)
        self.tsingnegemotion = self._read_ts_emotion_file(self.TsinghuaNegFile)
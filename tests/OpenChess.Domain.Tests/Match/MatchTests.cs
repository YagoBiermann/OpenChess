using OpenChess.Domain;

namespace OpenChess.Tests
{

    [TestClass]
    public class MatchTests
    {
        private static MatchInfo RestoreMatch(string mId, string p1Id, string p2Id, int mtime = 5, string mstatus = "InProgress", string? winner = null)
        {
            string matchId = mId;
            string player1Id = p1Id;
            string player2Id = p2Id;

            PlayerInfo player1 = new(player1Id, 'w', matchId);
            PlayerInfo player2 = new(player2Id, 'b', matchId);
            List<PlayerInfo> players = new() { player1, player2 };
            var status = mstatus;
            var time = mtime;
            List<string> pgnMoves = new() { "2. d5", "1. e4" };
            string fen = "rnbqkbnr/ppp1pppp/8/3p4/4P3/8/PPPP1PPP/RNBQKBNR w KQkq D6 0 1";
            var pgnStack = new Stack<string>(pgnMoves);
            MatchInfo matchInfo = new(matchId, players, fen, pgnStack, status, time, winner);

            return matchInfo;
        }

        private static Match RestoreAndPlay(string fen, string origin, string destination, string? promoting = null)
        {
            string matchId = Guid.NewGuid().ToString();
            PlayerInfo player1 = new(Guid.NewGuid().ToString(), 'w', matchId);
            PlayerInfo player2 = new(Guid.NewGuid().ToString(), 'b', matchId);
            List<PlayerInfo> players = new() { player1, player2 };
            MatchInfo matchInfo = new(matchId, players, fen, new(), MatchStatus.InProgress.ToString(), 5);
            Match match = new(matchInfo);
            Guid currentPlayer = match.CurrentPlayer!.Value;
            match.Play(new(currentPlayer, Coordinate.GetInstance(origin), Coordinate.GetInstance(destination), promoting));

            return match;
        }

        [DataRow(3)]
        [DataRow(5)]
        [DataRow(10)]
        [DataRow(15)]
        [DataRow(30)]
        [TestMethod]
        public void NewInstance_GivenTime_ShouldCreateNewMatch(int time)
        {
            Time timeEnum = (Time)Enum.ToObject(typeof(Time), time);
            Match match = new(timeEnum);

            Assert.IsNotNull(match.Id);
            Assert.IsFalse(match.IsFull());
            Assert.IsNull(match.CurrentPlayer);
            Assert.AreEqual(time, (int)match.Time);
        }

        [TestMethod]
        public void NewInstance_ShouldRestoreGameStateCorrectly()
        {
            MatchInfo matchInfo = RestoreMatch(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
            Match match = new(matchInfo);

            Assert.AreEqual(match.Id, matchInfo.MatchId);
            Assert.AreEqual(match.Time, Time.Five);
            Assert.AreEqual(match.Status, MatchStatus.InProgress);
            Assert.AreEqual(match.Chessboard, matchInfo.Fen);
            Assert.IsNull(match.Winner);
            CollectionAssert.AreEquivalent(match.Players, matchInfo.Players);
            CollectionAssert.AreEqual(match.Moves, matchInfo.PgnMoves);
        }

        [TestMethod]
        public void NewInstance_RestoringGameState_InvalidProperties_ShouldThrowException()
        {
            string matchId = Guid.NewGuid().ToString();
            string player1 = Guid.NewGuid().ToString();
            string player2 = Guid.NewGuid().ToString();
            Assert.ThrowsException<MatchException>(() => RestoreMatch(matchId, player1, player2, 6));
            Assert.ThrowsException<MatchException>(() => RestoreMatch(matchId, player1, player2, 5, "imProges"));
            Assert.ThrowsException<MatchException>(() => RestoreMatch(matchId, player1, "invalidId"));
            Assert.ThrowsException<MatchException>(() => RestoreMatch(matchId, "invalidId", player2));
            Assert.ThrowsException<MatchException>(() => RestoreMatch("invalidId", player1, player2));
            Assert.ThrowsException<MatchException>(() => RestoreMatch(matchId, player1, player2, 5, "InProgress", "invalidId"));
        }

        [TestMethod]
        public void Join_MatchNotFull_ShouldAssignPlayerToMatch()
        {
            Match match = new(Time.Ten);
            PlayerInfo player = new(Color.White);

            match.Join(player);

            Assert.IsTrue(match.HasPlayer());
            Assert.AreEqual(MatchStatus.NotStarted, match.Status);
            Assert.IsFalse(match.IsFull());
            Assert.IsNull(match.CurrentPlayer);
        }

        [TestMethod]
        public void Join_WhenAllPlayersJoined_ShouldChangeStatusAndCurrentPlayer()
        {
            Match match = new(Time.Ten);
            PlayerInfo whitePlayer = new(Color.White);
            PlayerInfo blackPlayer = new(Color.Black);

            match.Join(whitePlayer);
            match.Join(blackPlayer);

            Assert.AreEqual(MatchStatus.InProgress, match.Status);
            Assert.AreEqual(whitePlayer.Id, match.CurrentPlayer);
        }

        [TestMethod]
        public void Join_AddingSamePlayerTwice_ShouldThrowException()
        {
            Match match = new(Time.Ten);
            PlayerInfo player = new(Color.White);
            PlayerInfo fakePlayer = new(player.Id, Color.Black);

            match.Join(player);

            Assert.ThrowsException<MatchException>(() => match.Join(player));
            Assert.ThrowsException<MatchException>(() => match.Join(fakePlayer));
        }

        [TestMethod]
        public void Join_AddingPlayerThatIsAlreadyInAnotherMatch_ShouldThrowException()
        {
            Match match = new(Time.Ten);
            PlayerInfo player = new(Guid.NewGuid(), Color.White, null);
            match.Join(player);
            Match match2 = new(Time.Ten);

            Assert.ThrowsException<MatchException>(() => match2.Join(match.Players.FirstOrDefault()));
        }

        [TestMethod]
        public void Join_FullMatch_ShouldThrowException()
        {
            Match match = new(Time.Ten);
            PlayerInfo whitePlayer = new(Color.White);
            PlayerInfo blackPlayer = new(Color.Black);

            PlayerInfo otherPlayer = new(Color.White);

            match.Join(whitePlayer);
            match.Join(blackPlayer);

            Assert.ThrowsException<MatchException>(() => match.Join(otherPlayer));
        }

        [TestMethod]
        public void IsFull_ShouldReturnTrue()
        {
            Match match = new(Time.Ten);
            PlayerInfo whitePlayer = new(Color.White);
            PlayerInfo blackPlayer = new(Color.Black);

            match.Join(whitePlayer);
            match.Join(blackPlayer);

            Assert.IsTrue(match.IsFull());
        }

        [TestMethod]
        public void IsFull_ShouldReturnFalse()
        {
            Match match = new(Time.Ten);
            PlayerInfo whitePlayer = new(Color.White);

            match.Join(whitePlayer);

            Assert.IsFalse(match.IsFull());
        }

        [TestMethod]
        public void Play_ValidMove_ShouldBeHandled()
        {
            Match match = new(Time.Ten);
            match.Join(new(Color.White));
            match.Join(new(Color.Black));
            string initialPosition = match.Chessboard;
            Move move = new(match.CurrentPlayer!.Value, Coordinate.GetInstance("E2"), Coordinate.GetInstance("E4"));

            match.Play(move);

            Assert.AreNotEqual(match.Chessboard, initialPosition);
            Assert.AreNotEqual(match.CurrentPlayer, move.PlayerId);
        }

        [TestMethod]
        public void Play_MatchNotStarted_ShouldThrowException()
        {
            Match match = new(Time.Three);
            PlayerInfo player = new(Color.White);
            match.Join(player);
            Move move = new(player.Id, Coordinate.GetInstance("E2"), Coordinate.GetInstance("E4"));
            Assert.ThrowsException<MatchException>(() => match.Play(move));
        }

        [TestMethod]
        public void Play_PlayerNotInMatch_ShouldThrowException()
        {
            Match match = new(Time.Ten);
            PlayerInfo player1 = new(Color.White);
            PlayerInfo player2 = new(Color.Black);
            match.Join(player1);
            match.Join(player2);

            Move move = new(Guid.NewGuid(), Coordinate.GetInstance("E2"), Coordinate.GetInstance("E4"));
            Assert.ThrowsException<MatchException>(() => match.Play(move));
        }

        [TestMethod]
        public void Play_PlayerMakingMoveWhenItsNotTheTurn_ShouldThrowException()
        {
            Match match = new(Time.Ten);
            PlayerInfo player1 = new(Color.White);
            PlayerInfo player2 = new(Color.Black);
            match.Join(player1);
            match.Join(player2);

            Move move = new(player2.Id, Coordinate.GetInstance("E2"), Coordinate.GetInstance("E4"));
            Assert.ThrowsException<MatchException>(() => match.Play(move));
        }

        [TestMethod]
        public void Play_EmptyOrigin_ShouldThrowException()
        {
            Match match = new(Time.Ten);
            PlayerInfo player1 = new(Color.White);
            PlayerInfo player2 = new(Color.Black);
            match.Join(player1);
            match.Join(player2);

            Move move = new(player1.Id, Coordinate.GetInstance("E4"), Coordinate.GetInstance("E6"));
            Assert.ThrowsException<ChessboardException>(() => match.Play(move));
        }

        [TestMethod]
        public void Play_PlayerTryingToMoveOpponentPiece_ShouldThrowException()
        {
            Match match = new(Time.Ten);
            PlayerInfo player1 = new(Color.White);
            PlayerInfo player2 = new(Color.Black);
            match.Join(player1);
            match.Join(player2);

            Move move = new(player1.Id, Coordinate.GetInstance("E7"), Coordinate.GetInstance("E5"));
            Assert.ThrowsException<ChessboardException>(() => match.Play(move));
        }

        [TestMethod]
        public void Play_PlayerInSelfCheckAfterMove_ShouldThrowException()
        {
            Match match = new(Time.Ten);
            PlayerInfo player1 = new(Color.White);
            PlayerInfo player2 = new(Color.Black);
            match.Join(player1);
            match.Join(player2);

            List<Move> moves = new()
            {
                new(player1.Id, Coordinate.GetInstance("E2"), Coordinate.GetInstance("E4")),
                new(player2.Id, Coordinate.GetInstance("E7"), Coordinate.GetInstance("E5")),
                new(player1.Id, Coordinate.GetInstance("D1"), Coordinate.GetInstance("H5")),
                new(player2.Id, Coordinate.GetInstance("F7"), Coordinate.GetInstance("F5")),
            };
            string lastPosition = match.Chessboard;

            foreach (Move move in moves)
            {
                bool isLastMove = moves.Last() == move;
                if (isLastMove)
                {
                    Assert.ThrowsException<ChessboardException>(() => match.Play(move));
                }
                else
                {
                    match.Play(move);
                }
            }
            string currentPosition = match.Chessboard;

            Assert.AreEqual(lastPosition, currentPosition);
        }

        [TestMethod]
        public void Play_ShouldAddPGNMove()
        {
            Match match = new(Time.Ten);
            PlayerInfo player1 = new(Color.White);
            PlayerInfo player2 = new(Color.Black);
            match.Join(player1);
            match.Join(player2);

            Move move = new(player1.Id, Coordinate.GetInstance("E2"), Coordinate.GetInstance("E4"));
            match.Play(move);

            Assert.IsTrue(match.Moves.Any());
        }

        [TestMethod]
        public void Play_PgnMoveList_ShouldBeInRightPgnFormat()
        {
            Match match = new(Time.Ten);
            PlayerInfo player1 = new(Color.White);
            PlayerInfo player2 = new(Color.Black);
            match.Join(player1);
            match.Join(player2);

            List<Move> moves = new()
            {
                new(player1.Id, Coordinate.GetInstance("E2"), Coordinate.GetInstance("E4")),
                new(player2.Id, Coordinate.GetInstance("D7"), Coordinate.GetInstance("D5")),
                new(player1.Id, Coordinate.GetInstance("E4"), Coordinate.GetInstance("D5")),
                new(player2.Id, Coordinate.GetInstance("D8"), Coordinate.GetInstance("D5")),
                new(player1.Id, Coordinate.GetInstance("F1"), Coordinate.GetInstance("C4")),
                new(player2.Id, Coordinate.GetInstance("D5"), Coordinate.GetInstance("C4")),
                new(player1.Id, Coordinate.GetInstance("D2"), Coordinate.GetInstance("D3")),
                new(player2.Id, Coordinate.GetInstance("C4"), Coordinate.GetInstance("D3")),
                new(player1.Id, Coordinate.GetInstance("C2"), Coordinate.GetInstance("D3")),
            };
            foreach (Move move in moves)
            {
                match.Play(move);
            }

            List<string> expectedMoveList = new()
            {
                "9. cxd3",
                "8. Qxd3",
                "7. d3",
                "6. Qxc4",
                "5. Bc4",
                "4. Qxd5",
                "3. exd5",
                "2. d5",
                "1. e4",
            };

            CollectionAssert.AreEqual(expectedMoveList, match.Moves);
        }

        [TestMethod]
        public void Play_ShouldHandleEnPassantMoves()
        {
            Match match = new(Time.Ten);
            PlayerInfo player1 = new(Color.White);
            PlayerInfo player2 = new(Color.Black);
            match.Join(player1);
            match.Join(player2);

            List<Move> moves = new()
            {
                new(player1.Id, Coordinate.GetInstance("E2"), Coordinate.GetInstance("E4")),
                new(player2.Id, Coordinate.GetInstance("D7"), Coordinate.GetInstance("D5")),
                new(player1.Id, Coordinate.GetInstance("E4"), Coordinate.GetInstance("E5")),
                new(player2.Id, Coordinate.GetInstance("F7"), Coordinate.GetInstance("F5")),
                new(player1.Id, Coordinate.GetInstance("E5"), Coordinate.GetInstance("F6")),
            };
            foreach (Move move in moves)
            {
                match.Play(move);
            }
        }

        [DataRow("rnbk1bnr/pp1Ppppp/1qp5/8/8/8/PPPP1PPP/RNBQKBNR w KQkq - 0 1", "D7", "C8", "Q")]
        [TestMethod]
        public void Play_PromotingPawn_ShouldAddPgnMoveProperly(string fen, string origin, string destination, string promoting)
        {
            Match match = RestoreAndPlay(fen, origin, destination);
            Assert.AreEqual("1. dxc8=Q", match.Moves.Peek());
        }

        [DataRow("r3k2r/pppq1ppp/2np1n2/1Bb1p1B1/4P1b1/2NP1N2/PPPQ1PPP/R3K2R b KQkq - 0 1", "E8", "G8")]
        [DataRow("r3k2r/pppq1ppp/2np1n2/1Bb1p1B1/4P1b1/2NP1N2/PPPQ1PPP/R3K2R w KQkq - 0 1", "E1", "G1")]
        [DataRow("r3k2r/pppq1ppp/2np1n2/1Bb1p1B1/4P1b1/2NP1N2/PPPQ1PPP/R3K2R b KQkq - 0 1", "E8", "C8")]
        [DataRow("r3k2r/pppq1ppp/2np1n2/1Bb1p1B1/4P1b1/2NP1N2/PPPQ1PPP/R3K2R w KQkq - 0 1", "E1", "C1")]
        [TestMethod]
        public void Play_Castling_ShouldAddPgnMoveProperly(string fen, string origin, string destination)
        {
            Match match = RestoreAndPlay(fen, origin, destination);
            string castlingPgn = destination[0] == 'G' ? "O-O" : "O-O-O";

            Assert.AreEqual(castlingPgn, match.Moves.Peek());
        }
    }
}
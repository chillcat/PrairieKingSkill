using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrairieKingSkill
{
    class ProfessionTree
    {
        public class ProfessionTreeBuilder
        {
            private Profession root;
            private Profession right;
            private Profession left;

            public ProfessionTreeBuilder Root(Profession root)
            {
                this.root = root;
                return this;
            }

            public ProfessionTreeBuilder Right(Profession right)
            {
                this.right = right;
                return this;
            }

            public ProfessionTreeBuilder Left(Profession left)
            {
                this.left = left;
                return this;
            }

            public ProfessionTree build()
            {
                if (root != null && right != null && left != null)
                {
                    return new ProfessionTree(root, right, left);
                } else
                {
                    throw new Exception("Cannot initialize ProfessionTree, all fields are required.");
                }
            }

        }

        public static ProfessionTreeBuilder builder()
        {
            return new ProfessionTreeBuilder();
        }

        private ProfessionTree(Profession root, Profession right, Profession left)
        {
            Root = root;
            Right = right;
            Left = left;
        }
        public Profession Root { get; private set; }
        public Profession Right { get; private set; }
        public Profession Left { get; private set; }
    }
}
